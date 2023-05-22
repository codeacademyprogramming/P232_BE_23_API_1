namespace AdminUI.ViewModels
{
    public class ErrorReponseViewModel
    {
        public string Message { get; set; } 
        public List<ErrorReponseItemViewModel> Errors { get; set; }
    }
    public class ErrorReponseItemViewModel
    {
        public string Key { get; set; }
        public string Message { set; get; }
    }
}
