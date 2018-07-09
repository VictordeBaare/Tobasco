using System.Linq;

namespace Tobasco.Generation
{
    public class GenerationOptions
    {
        /// <summary>
        /// Time consuming action in which the whole solution will be check for unused .txt4 placeholder files.
        /// If any of these are found they will be removed together with their child items.
        /// </summary>
        public bool CleanUnusedTxt4Files { get; set; } = true;

        /// <summary>
        /// A list of the entity names that you want to generate. If left empty all xmls will be generated.
        /// </summary>
        public string[] EntitiesToGenerate { get; set; } = new string[] { };

        public bool GenerateSubSet { get { return EntitiesToGenerate.Any(); } }
    }
}
