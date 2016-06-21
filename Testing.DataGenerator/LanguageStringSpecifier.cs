namespace PeinearyDevelopment.Framework.BaseClassLibraries.Testing.DataGenerator
{
    public class LanguageStringSpecifier
    {
        public LanguageStringSpecifier()
        {
            TokenBeginners = string.Empty;
            TokenFillers = string.Empty;
            TokenEnders = string.Empty;
        }

        public string TokenBeginners { get; set; }
        public string TokenFillers { get; set; }
        public string TokenEnders { get; set; }
    }
}
