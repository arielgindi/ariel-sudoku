ariel-sudoku


Sudoku solver that was made to solve any given sudoku in less than a second!
The heart of the algorithm is backtracking + Heuristics
Sizes (1x1,4x4,9x9,16x16,25x25) are supported

Algorithm:

The main key to get high level of performance is that when you have to take a guess, take the best guess you can! 
Because on every wrong guess it can lead into a big path of wrong guess, and it can take a lot of time until detecting that, in the meanwhile gold time is spent.
If you make an wrong guess, make sure that you will know that as ASAP because it can lead to even more wrong guesses.

Backtracking - 2 threads are working simultaneously, each of them is a different algorithm competing to solve the puzzle first, the algorithms are very similar, one pick both pick the cell with the least amount of possibilities when they have to take a guess, but the second one pick the cell that also has the last amount of neighbors that aren't empty. 
after the guess instantly check if there is an dead end, if found try another guess!
The human tactics are running as long as they have options.

Human tactics (Heuristics) - 

only two of them used
Hidden singles - if a cell contain 1 possibilities place it
Naked singles - if a digit is the only one is a certain unit (row,col,box) instantly place it!

Hidden singles run as long as it run out of options (because he is the fastest) if no more hidden singles found, continue to Naked singles, if naked singles placed a digit go back instantly to Hidden single (again, because he is the fastest) otherwise go back to Backtrack and try the next guess!

Dead end detection - Highly optimised dead end detection, 
it check
1) If an empty cell that has contain zero possibilities -> dead end
2) Is there a number that has nowhere to go in a (row,col,box) -> dead end





Multithreading - As been said in the backtracking section, both thread run different algorithms to compete which one could solve the board the quickest. also it was used in this project because each algorithm of both of then has his weekends, each one of them have specific puzzles which cannot be solved in a realistic time, it was chosen to pick both! so if one has a weakness or is slower in 'easy' puzzles let them compete.


Precomputed data - Each time a puzzle is computed a set of constants are computed only once, for example the solver stores each cell neighbours, because its something that could be computed only once, (its the same always) its being calculated only once. each size of board has a cached (1x1,4x4,9x9,16x16,25x25) so it's being calculated only once in each runtime, for example if I will run 10 9x9 puzzle and 1 25x25 puzzle. there will be only 2 cached precomputed data (9x9 and 25x25)
