﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    public interface IAdventPuzzle
    {
        public string Name { get; }

        public PuzzleOutput GetOutput();
    }
}
