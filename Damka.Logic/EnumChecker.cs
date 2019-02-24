using System;
using System.Collections.Generic;
using System.Text;

namespace Damka.Logic
{
    public enum eCheckerGame
    {
        WhitePlayer = 'O',
        WhiteKing = 'U',
        BlackPlayer = 'X',
        BlackKing = 'K',
    }

    public enum eBoardSize
    {
        BoardSizeSix = 6,
        BoardSizeEight = 8,
        BoardSizeTen = 10
    }

    public enum eGameUserChoise
    {
        k_Human = 1,
        k_PC = 2
    }
}
