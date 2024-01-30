namespace GameSence.World.Game
{
    /// <summary>
    /// 小游戏奖励专用
    /// </summary>
    public class Award
    {
        public Award(string cardName, int score)
        {
            CardName = cardName;
            Score = score;
        }

        public string CardName;
        public int Score;
    }
}