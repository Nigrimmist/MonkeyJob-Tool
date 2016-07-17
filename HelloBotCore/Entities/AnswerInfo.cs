using System.Drawing;
using System.Net.Mime;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class AnswerInfo
    {
        public CommunicationMessage Answer { get; set; }
        public string Title { get; set; }
        public AnswerBehaviourType AnswerType { get; set; }
        public string CommandName { get; set; }
        public ModuleType MessageSourceType { get; set; }
        public Image Icon { get; set; }
        public Color? HeaderBackgroundColor { get; set; }
        public Color? BodyBackgroundColor { get; set; }
    }
}
