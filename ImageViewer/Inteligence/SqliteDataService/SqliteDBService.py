import sqlite3
import json
from typing import Any, Dict, List, Optional


class ImageMetaDB:
    def __init__(self, db_path: str = "images.db"):
        """
        Initialize DB connection
        """
        self.db_path = db_path
        self._create_tables()

    # -----------------------------
    # Connection helper
    # -----------------------------
    def _connect(self):
        return sqlite3.connect(self.db_path)

    # -----------------------------
    # Create tables
    # -----------------------------
    def _create_tables(self):
        conn = self._connect()
        cursor = conn.cursor()

        cursor.execute("""
        CREATE TABLE IF NOT EXISTS image_meta (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            image_path TEXT NOT NULL,
            image_name TEXT NOT NULL,
            json_data TEXT,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );
        """)

        conn.commit()
        conn.close()

    # -----------------------------
    # Insert image meta
    # -----------------------------
    def insert_image(
        self,
        image_path: str,
        image_name: str,
        json_data: Dict[str, Any]
    ) -> int:
        """
        Insert image metadata + huge JSON
        Returns inserted row id
        """
        conn = self._connect()
        cursor = conn.cursor()

        cursor.execute("""
        INSERT INTO image_meta
        (image_path, image_name, json_data)
        VALUES (?, ?, ?)
        """, (
            image_path,
            image_name,
            json.dumps(json_data)
        ))

        conn.commit()
        row_id = cursor.lastrowid
        conn.close()

        return row_id

    # -----------------------------
    # Bulk insert
    # -----------------------------
    def bulk_insert(
        self,
        records: List[Dict[str, Any]]
    ):
        """
        Insert multiple records
        records = [
            {
                "image_path": "...",
                "image_name": "...",
                "json_data": {...}
            }
        ]
        """
        conn = self._connect()
        cursor = conn.cursor()

        data = [
            (
                r["image_path"],
                r["image_name"],
                json.dumps(r.get("json_data", {}))
            )
            for r in records
        ]

        cursor.executemany("""
        INSERT INTO image_meta
        (image_path, image_name, json_data)
        VALUES (?, ?, ?)
        """, data)

        conn.commit()
        conn.close()

    # -----------------------------
    # Get all images
    # -----------------------------
    def get_all(self) -> List[Dict[str, Any]]:
        conn = self._connect()
        cursor = conn.cursor()

        cursor.execute("SELECT * FROM image_meta")

        columns = [c[0] for c in cursor.description]

        results = []
        for row in cursor.fetchall():
            record = dict(zip(columns, row))

            if record["json_data"]:
                record["json_data"] = json.loads(
                    record["json_data"]
                )

            results.append(record)

        conn.close()
        return results

    # -----------------------------
    # Get by ID
    # -----------------------------
    def get_by_id(
        self,
        image_id: int
    ) -> Optional[Dict[str, Any]]:

        conn = self._connect()
        cursor = conn.cursor()

        cursor.execute(
            "SELECT * FROM image_meta WHERE id=?",
            (image_id,)
        )

        row = cursor.fetchone()

        if not row:
            return None

        columns = [c[0] for c in cursor.description]
        record = dict(zip(columns, row))

        if record["json_data"]:
            record["json_data"] = json.loads(
                record["json_data"]
            )

        conn.close()
        return record

    # -----------------------------
    # Update JSON data
    # -----------------------------
    def update_json(
        self,
        image_id: int,
        json_data: Dict[str, Any]
    ):
        conn = self._connect()
        cursor = conn.cursor()

        cursor.execute("""
        UPDATE image_meta
        SET json_data=?
        WHERE id=?
        """, (
            json.dumps(json_data),
            image_id
        ))

        conn.commit()
        conn.close()

    # -----------------------------
    # Delete record
    # -----------------------------
    def delete(self, image_id: int):
        cursor = conn.cursor()
        conn = self._connect()

        cursor.execute(
            "DELETE FROM image_meta WHERE id=?",
            (image_id,)
        )

        conn.commit()
        conn.close()

    def exist(self,image_path:str)->bool:
        conn = self._connect()
        cursor = conn.cursor()
        cursor.execute(
            "SELECT id FROM image_meta WHERE image_path=?",
            (image_path,)
        )
        row = cursor.fetchone()
        conn.close()
        return row is not None
