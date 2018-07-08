namespace Tobasco.Generation
{
    public class GenerationOptions
    {
        public bool CleanUnusedTxt4Files { get; set; } = true;

        public string[] EntitiesToGenerate { get; set; } = new string[] { };

        
    }
}
