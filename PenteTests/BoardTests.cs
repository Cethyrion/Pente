﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pente;

namespace PenteTests
{
    [TestClass]
    public class BoardTests
    {
        private readonly Piece[] successfulCapture = { Piece.P1, Piece.EMPTY, Piece.EMPTY, Piece.P1 };
        private Vec2 start = new Vec2(9, 9);

        #region Initialize Tests
        [TestMethod]
        public void BoardInitialize_Players_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            board.Initialize(p1, p2, 19);

            Assert.IsTrue((p1 == board.p1) && (p2 == board.p2));
        }

        [TestMethod]
        public void BoardInitialize_Grid_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            CheckProperInitialization(board, p1, p2, 19);
            CheckProperInitialization(board, p1, p2, 9);
            CheckProperInitialization(board, p1, p2, 31);
        }

        private static void CheckProperInitialization(Board board, Player p1, Player p2, int size)
        {
            board.Initialize(p1, p2, size);

            int xLength = board.Grid.GetLength(0);
            int yLength = board.Grid.GetLength(1);

            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    Assert.AreEqual((i == xLength / 2 && j == yLength / 2) ? Piece.P1 : Piece.EMPTY, board.Grid[i, j]);
                }
            }
        }

        [TestMethod]
        public void BoardInitialize_Grid_ReInit_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            board.Grid.Set(start, Piece.P2);

            CheckProperInitialization(board, p1, p2, 19);

            board.Grid.Set(start, Piece.P2);

            CheckProperInitialization(board, p1, p2, 11);

            board.Grid.Set(start, Piece.P2);

            CheckProperInitialization(board, p1, p2, 27);
        }

        [TestMethod]
        public void BoardInitialize_Grid_Dimensions_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            for (int i = 9; i < 40; i += 2)
            {
                board.Initialize(p1, p2, i);
                Assert.AreEqual(i, board.Grid.GetLength(0));
                Assert.AreEqual(i, board.Grid.GetLength(1));
            }
        }

        [TestMethod]
        public void BoardInitialize_TurnCount_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();


            board.turnCount = 5;

            board.Initialize(p1, p2, 19);
            Assert.AreEqual(1, board.turnCount);


            board.turnCount = int.MaxValue;

            board.Initialize(p1, p2, 9);
            Assert.AreEqual(1, board.turnCount);


            board.turnCount = int.MinValue;

            board.Initialize(p1, p2, 39);
            Assert.AreEqual(1, board.turnCount);
        }
        #endregion

        #region Capture Checks
        public delegate bool CheckForDelegate(Vec2 position);

        public void BoardTestCheckForPatternMethod(ref Board board, Vec2 direction, Piece[] pattern, Piece playerPiece, CheckForDelegate checkForDelegate)
        {
            Player p1 = new Player();
            Player p2 = new Player();

            pattern = pattern.GetPatternFor(playerPiece);

            board.Initialize(p1, p2, 19);
            board.turnCount = playerPiece == Piece.P1 ? 0 : 1;

            for (int i = 0; i < pattern.Length; i++)
            {
                board.Grid.Set(start + direction * i, pattern[i]);
            }

            Assert.IsTrue(checkForDelegate(start));
        }

        private void PatternExists(Board board, Vec2 direction, Piece[] patternToFind)
        {
            for (int i = 0; i < patternToFind.Length; i++)
            {
                Assert.AreEqual(patternToFind[i], board.Grid.Get(start + direction * i));
            }
        }

        [TestMethod]
        public void BoardCheckForCapture_Vertical_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 0, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_Horizontal_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 0 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_DiagonalDown_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_DiagonalUp_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = -1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P1, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_Vertical_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 0, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_Horizontal_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 0 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_DiagonalDown_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }

        [TestMethod]
        public void BoardCheckForCapture_DiagonalUp_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = -1 };

            BoardTestCheckForPatternMethod(ref board, direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Capture, Piece.P2, board.CheckForCapture);

            PatternExists(board, direction, successfulCapture);
        }
        #endregion

        #region Win Checks
        [TestMethod]
        public void BoardCheckForWin_FiveCaptures_P1_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            board.Initialize(p1, p2, 19);
            board.p1.Captures = 5;

            Assert.IsTrue(board.CheckForWin(start));
        }

        [TestMethod]
        public void BoardCheckForWin_FiveCaptures_P2_Test()
        {
            Board board = new Board();
            Player p1 = new Player();
            Player p2 = new Player();

            board.Initialize(p1, p2, 19);
            board.turnCount = 1;
            board.p2.Captures = 5;

            Assert.IsTrue(board.CheckForWin(start));
        }

        [TestMethod]
        public void BoardCheckForWin_Vertical_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 0, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P1, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P1, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_Horizontal_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 0 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P1, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P1, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_DiagonalDown_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P1, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P1, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_DiagonalUp_P1_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = -1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P1, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P1, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_Vertical_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 0, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P2, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P2, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_Horizontal_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 0 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P2, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P2, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_DiagonalDown_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2 { x = 1, y = 1 };
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P2, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P2, board.CheckForWin);
        }

        [TestMethod]
        public void BoardCheckForWin_DiagonalUp_P2_Test()
        {
            Board board = new Board();

            //forwards
            Vec2 direction = new Vec2(1, -1);
            BoardTestCheckForPatternMethod(ref board, direction, board.Win, Piece.P2, board.CheckForWin);

            //backwards
            BoardTestCheckForPatternMethod(ref board, -direction, board.Win, Piece.P2, board.CheckForWin);
        }
        #endregion
    }
}