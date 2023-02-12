namespace WorldGame
{
    /// <summary>
    /// 小游戏奖励专用
    /// </summary>
    public class Award
    {
        public Award(string cardName, int score)
        {
            this.CardName = cardName;
            this.Score = score;
        }

        public string CardName;
        public int Score;
    }
}