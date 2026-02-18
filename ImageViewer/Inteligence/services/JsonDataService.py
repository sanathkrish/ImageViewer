import json
import os
import threading
import time
import atexit
from datetime import datetime


class JsonDataService:

    def __init__(
        self,
        file_path,
        auto_flush_interval=100,  # 5 min
        enable_backup=False
    ):
        self.file_path = file_path
        self.lock = threading.Lock()
        self.data = {}
        self.enable_backup = enable_backup

        self._load()

        # Auto flush thread
        self.interval = auto_flush_interval
        self._start_auto_flush()

        # Flush on app exit
        atexit.register(self.flush)

    # -----------------------------
    # Load JSON
    # -----------------------------
    def _load(self):

        if os.path.exists(self.file_path):
            with open(self.file_path, "r") as f:
                self.data = json.load(f)
        else:
            self.data = {}

    # -----------------------------
    # Get value
    # -----------------------------
    def get(self, key, default=None):
        return self.data.get(key, default)

    # -----------------------------
    # Set value (buffered)
    # -----------------------------
    def set(self, key, value):
        with self.lock:
            self.data[key] = value

    # -----------------------------
    # Nested update
    # -----------------------------
    def update_nested(
        self,
        parent_key,
        child_key,
        value
    ):
        with self.lock:

            if parent_key not in self.data:
                self.data[parent_key] = {}

            self.data[parent_key][child_key] = value

    # -----------------------------
    # Bulk insert
    # -----------------------------
    def bulk_set(self, items: dict):
        with self.lock:
            self.data.update(items)

    # -----------------------------
    # Delete key
    # -----------------------------
    def delete(self, key):
        with self.lock:
            if key in self.data:
                del self.data[key]

    # -----------------------------
    # Flush to disk
    # -----------------------------
    def flush(self):

        with self.lock:

            # Backup old version
            if (
                self.enable_backup and
                os.path.exists(self.file_path)
            ):
                ts = datetime.now().strftime(
                    "%Y%m%d_%H%M%S")

                backup =   f"{self.file_path}.{ts}.bak"

                os.rename(
                    self.file_path,
                    backup
                )

            # Write file
            with open(self.file_path, "w") as f:
                json.dump(
                    self.data,
                    f,
                    indent=2
                )

        print("Flushed to disk")

    # -----------------------------
    # Auto flush loop
    # -----------------------------
    def _start_auto_flush(self):

        def loop():
            while True:
                time.sleep(self.interval)
                self.flush()

        t = threading.Thread(
            target=loop,
            daemon=True
        )
        t.start()
