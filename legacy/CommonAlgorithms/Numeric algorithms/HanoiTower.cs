using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.Numerical
{
    public class HanoiTower
    {
        public HanoiTower(ushort numberOfDisks)
        {
            disksCount = numberOfDisks;
            pegs = new Stack<Disk>[3];
            pegs[0] = new Stack<Disk>(numberOfDisks);
            pegs[1] = new Stack<Disk>(numberOfDisks);
            pegs[2] = new Stack<Disk>(numberOfDisks);

            for (ushort i = numberOfDisks; i > 0; i--)
                pegs[0].Push(new Disk(i));
        }

        private ushort disksCount;
        public ushort DisksCount { get { return disksCount; } }
        public Stack<Disk>[] pegs;
        public event DiskMovedDelegate DiskMoved;

        /// <summary>
        /// Recursively move n disks from fromPeg to toPeg
        /// </summary>
        public void Solve(ushort fromPeg, ushort toPeg, ushort n)
        {
            if (fromPeg < 1 || fromPeg > 3) throw new InvalidOperationException("Valid peg numbers are: 1,2,3");
            if (fromPeg == toPeg) throw new InvalidOperationException("Pegs are the same");
            if (n > pegs[fromPeg-1].Count) throw new InvalidOperationException(string.Format("Number of disks to move from peg {0} exceeds total disks count of the peg", n));
            ushort otherPeg = GetOtherPeg(fromPeg, toPeg);

            if (n == 1)                                         // move 1 disk from [fromPeg] to [toPeg]
            {
                var disk = pegs[fromPeg - 1].Pop();
                pegs[toPeg - 1].Push(disk);                
                if (DiskMoved != null) DiskMoved.Invoke(disk.DiskNumber);
                return;
            }
            if (n > 1)      
            {
                Solve(fromPeg, otherPeg, (ushort)(n - 1));      // move recurisvely n-1 disks from [fromPeg] to [otherPeg]      
                Solve(fromPeg, toPeg, 1);                       // move 1 disk from [fromPeg] to [toPeg]
                Solve(otherPeg, toPeg, (ushort)(n - 1));        // move n-1 disks from [otherPeg] back to [toPeg]
            }
        }
        /// <summary>
        /// Returns remaining peg's index given 2 other pegs
        /// </summary>
        private ushort GetOtherPeg(ushort peg1, ushort peg2)
        {
            if (peg1 == peg2) throw new InvalidOperationException("Pegs are the same");
            if (peg1 + peg2 == 3) return 3;     //1+2
            if (peg1 + peg2 == 4) return 2;     //1+3
            return 1;                           //3+2
        }
        public class Disk
        {
            public Disk(ushort number)
            {
                this.number = number;
            }
            private ushort number;
            public ushort DiskNumber
            {
                get { return number; }
            }
        }
        public delegate void DiskMovedDelegate(ushort n);
    }
}
