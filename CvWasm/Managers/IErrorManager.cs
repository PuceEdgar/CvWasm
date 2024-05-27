namespace CvWasm.Managers;

public interface IErrorManager
{
    string FailedToLoadCvMessage { get; }
    string FailedToLoadCommandDescriptionMessage { get; }
    string FailedToLoadAsciiArtMessage { get; }
    string GenerateBadCommandErrorMessage(string command, Languages currentLanguage);
}
