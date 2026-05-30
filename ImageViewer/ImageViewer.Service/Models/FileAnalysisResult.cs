namespace ImageViewer.Service.Models
{
    public class FileAnalysisResult
    {
        public int FileId { get; set; }
        public string Path { get; set; }
        public bool IsDuplicate { get; set; }
        public int? DuplicateOf { get; set; }
        public bool IsSimilar { get; set; }
        public int? SimilarTo { get; set; }
        public bool IsBlurred { get; set; }
        public bool IsCorrupted { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
