namespace CommandPattern.Core.Game;

// giver værdi lighed gratis, fx new Position(1, 2) == new Position(1, 2) er true
public readonly record struct Position(int X, int Y);
