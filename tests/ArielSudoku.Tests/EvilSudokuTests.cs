﻿namespace ArielSudoku.Tests;
public class EvilSudokuTests : SudokuTestsBase
{
    [Fact]
    public void SolveEvilSudoku1()
    {
        string puzzle = "098000470001304200200000005000000000005293700060000040050060030100807004300409007";
        string expectedSolution = "698521473571384296234976815712645389845293761963718542457162938129837654386459127";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }

    [Fact]
    public void SolveEvilSudoku2()
    {
        string puzzle = "060000030208000504700000002900605003600429001000080000000060000096000720540208096";
        string expectedSolution = "164852937238796514759134682982615473673429851415387269827963145396541728541278396";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }

    [Fact]
    public void SolveEvilSudoku3()
    {
        string puzzle = "000206000601040020720003054000002005072000360500700000250300017030070209000401000";
        string expectedSolution = "345296178681547923729813654968132745472958361513764892256389417134675289897421536";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }

    
    [Fact]
    public void SolveEvilSudoku4()
    {
        string puzzle = "006008000300050001402090005040002106000901000601400020500010803800040002000800900";
        string expectedSolution = "156238497398754261472196385947582136235961748681473529524619873819347652763825914";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }

    [Fact]
    public void SolveEvilSudoku5()
    {
        string puzzle = "050908600800006007006020000009000070203000809010000400000030700900800004005604030";
        string expectedSolution = "357948621821356947496721385549183276273465819618279453164532798932817564785694132";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }


    [Fact]
    public void SolveEvilSudoku6()
    {
        // Difficulty: Super hard for backtraking
        string puzzle = "050908600800006007006020000009000070203000809010000400000030700900800004005604030";
        string expectedSolution = "357948621821356947496721385549183276273465819618279453164532798932817564785694132";
        CheckPuzzleSolution(puzzle, expectedSolution);
    }
}