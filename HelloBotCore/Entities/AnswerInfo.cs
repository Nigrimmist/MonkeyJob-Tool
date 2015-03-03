using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class AnswerInfo
    {
        public string Answer { get; set; }
        public AnswerBehaviourType Type { get; set; }
        public string Command { get; set; }
    }
}
