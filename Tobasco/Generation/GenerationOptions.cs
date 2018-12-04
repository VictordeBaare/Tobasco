using System.Linq;

namespace Tobasco.Generation
{
    public class GenerationOptions
    {
        /// <summary>
        /// A list of the entity names that you want to generate. If left empty all xmls will be generated.
        /// </summary>
        public bool ForceCleanAndGenerate { get; set; }        
    }
}
