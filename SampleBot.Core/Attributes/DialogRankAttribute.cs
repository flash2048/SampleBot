using System;
namespace SampleBot.Core.Attributes
{
    public class DialogRankAttribute : Attribute
    {
        public DialogRankAttribute(int rank = 0)
        {
            Rank = rank;
        }

        public int Rank { get; }
    }
}
