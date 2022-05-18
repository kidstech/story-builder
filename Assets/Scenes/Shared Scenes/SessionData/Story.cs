using System.Collections.Generic;

namespace ServerTypes
{
    ///<summary>
    /// Stories created by users in storybuilder view
    ///</summary>
    public class Story
    {
        public string learnerId;
        public string storyName;
        public string font;
        public List<StoryPage> pages;

        public Story(List<StoryPage> storyPages)
        {
            pages = storyPages;
        }

    }
    public class StoryPage
    {
        ///<summary>
        /// String of all the text on the sentences on a page
        ///</summary>
        public List<string> sentences;
        public int pageNumber;

        public StoryPage(List<string> list, int number)
    {
        sentences = list;
        pageNumber = number;
    }

    }
}