using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class AnswerInfo
    {
        public string Answer { get; set; }
        public string Title { get; set; }
        public AnswerBehaviourType AnswerType { get; set; }
        public string CommandName { get; set; }
        public ModuleType MessageSourceType { get; set; }
    }
}
