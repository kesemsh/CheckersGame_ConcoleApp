namespace Ex02_Checkers
{
    public static class InputStringChecker
    {
        private const string k_Exit = "Q";
        private const string k_HumanPlayer = "1";
        private const string k_ComputerPlayer = "2";
        private const string k_PlayAgain = "1";
        private const string k_DontPlayAgain = "2";

        public static bool IsBoardSizeLegal(string i_InputSize, out eBoardSizes o_Size)
        {
            bool isBoardSizeValid = int.TryParse(i_InputSize, out int intSize);

            o_Size = (eBoardSizes)intSize;

            return (isBoardSizeValid == true) && (o_Size == eBoardSizes.SmallSize || o_Size == eBoardSizes.MediumSize || o_Size == eBoardSizes.LargeSize);
        }

        public static bool IsPlayerNameLegal(string i_PlayerName)
        {
            const int maxLenght = 20;
            const int minLenght = 1;

            return i_PlayerName.Length <= maxLenght && i_PlayerName.Length >= minLenght && i_PlayerName.Contains(" ") == false;
        }

        public static bool IsPlayerWantExit(string i_Input)
        {
            return i_Input.Equals(k_Exit);
        }

        public static bool IsPlayerTypeLegal(string i_PlayerType)
        {
            bool isPlayerTypeValid = true;

            if (IsPlayerTypeAHuman(i_PlayerType) == false && IsPlayerTypeAComputer(i_PlayerType) == false)
            {
                isPlayerTypeValid = false;
            }

            return isPlayerTypeValid;
        }

        public static bool IsPlayerTypeAComputer(string i_Input)
        {
            return i_Input.Equals(k_ComputerPlayer);
        }

        public static bool IsPlayerTypeAHuman(string i_Input)
        {
            return i_Input.Equals(k_HumanPlayer);
        }

        public static bool IsPlayAgainAnswerLegal(string i_PlayAgainAnswer)
        {
            bool isPlayAgainAnswerValid = true;

            if (IsAnswerPlayeAgain(i_PlayAgainAnswer) == false && IsAnswerDontPlayeAgain(i_PlayAgainAnswer) == false)
            {
                isPlayAgainAnswerValid = false;
            }

            return isPlayAgainAnswerValid;
        }

        public static bool IsAnswerPlayeAgain(string i_Input)
        {
            return i_Input.Equals(k_PlayAgain);
        }

        public static bool IsAnswerDontPlayeAgain(string i_Input)
        {
            return i_Input.Equals(k_DontPlayAgain);
        }
    }
}
