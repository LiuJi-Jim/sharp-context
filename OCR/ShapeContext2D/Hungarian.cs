using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        private const int EndOfPath = -0;
        private const int NoMatch = -10000000;

        private static bool isNoMatch(int i) {
            return i == NoMatch || i == -NoMatch;
        }

        private static bool isNoMatch(double i) {
            return i == NoMatch || i == -NoMatch;
        }

        private static bool isNotNoMatch(int i) {
            return !isNoMatch(i);
        }

        private static bool isNotNoMatch(double i) {
            return !isNoMatch(i);
        }

        public int[] Hungarian(Matrix A, out double T) {
            //function [C,T]=hungarian(A)
            //%HUNGARIAN Solve the Assignment problem using the Hungarian method.
            //%
            //%[C,T]=hungarian(A)
            //%A - a square cost matrix.
            //%C - the optimal assignment.
            //%T - the cost of the optimal assignment.

            //% Adapted from the FORTRAN IV code in Carpaneto and Toth, "Algorithm 548:
            //% Solution of the assignment problem [H]", ACM Transactions on
            //% Mathematical Software, 6(1):104-111, 1980.

            //% v1.0  96-06-14. Niclas Borlin, niclas@cs.umu.se.
            //%                 Department of Computing Science, Ume?University,
            //%                 Sweden. 
            //%                 All standard disclaimers apply.

            //% A substantial effort was put into this code. If you use it for a
            //% publication or otherwise, please include an acknowledgement or at least
            //% notify me by email. /Niclas

            //[m,n]=size(A);
            int m = A.Rows, n = A.Columns;

            //if (m~=n)
            //    error('HUNGARIAN: Cost matrix must be square!');
            //end
            if (m != n) {
                throw new Exception("匈牙利算法只适用于方阵");
            }

            //% Save original cost matrix.
            //orig=A;
            var orig = A.Clone();

            //% Reduce matrix.
            //A=hminired(A);
            A = Hminired(A);

            //% Do an initial assignment.
            //[A,C,U]=hminiass(A);
            int[] C, U;
            Hminiass(A, out C, out U);

            //% Repeat while we have unassigned rows.
            //while (U(n+1))
            while (U[n] >= 0) {
                //    % Start with no path, no unchecked zeros, and no unexplored rows.
                //    LR=zeros(1,n);
                var LR = Utils.InitArray<int>(n, NoMatch);
                //    LC=zeros(1,n);
                var LC = Utils.InitArray<int>(n, NoMatch);
                //    CH=zeros(1,n);
                var CH = Utils.InitArray<int>(n, NoMatch);
                //    RH=[zeros(1,n) -1];
                var RH = Utils.InitArray<int>(n + 1, NoMatch);
                RH[n] = EndOfPath; // TODO:不确定是-2还是0

                //    % No labelled columns.
                //    SLC=[];
                var SLC = new List<int>();

                //    % Start path in first unassigned row.
                //    r=U(n+1);
                int r = U[n];
                //    % Mark row with end-of-path label.
                //    LR(r)=-1;
                LR[r] = EndOfPath;
                //    % Insert row first in labelled row set.
                //    SLR=r;
                var SLR = new List<int>();
                SLR.Add(r);

                //    % Repeat until we manage to find an assignable zero.
                //    while (1)
                while (true) {
                    int l;
                    //        % If there are free zeros in row r
                    //        if (A(r,n+1)~=0)
                    if (isNotNoMatch(A[r, n])) {
                        //            % ...get column of first free zero.
                        //            l=-A(r,n+1);
                        l = -(int)A[r, n];

                        //            % If there are more free zeros in row r and row r in not
                        //            % yet marked as unexplored..
                        //            if (A(r,l)~=0 & RH(r)==0)
                        if (isNotNoMatch(A[r, l]) && isNoMatch(RH[r])) {

                            //                % Insert row r first in unexplored list.
                            //                RH(r)=RH(n+1);
                            RH[r] = RH[n];
                            //                RH(n+1)=r;
                            RH[n] = r;

                            //                % Mark in which column the next unexplored zero in this row
                            //                % is.
                            //                CH(r)=-A(r,l);
                            CH[r] = -(int)A[r, l];
                            //            end
                        }
                        //        else
                    } else {
                        //            % If all rows are explored..
                        //            if (RH(n+1)<=0)
                        if (RH[n] <= 0) { // TODO 不知道
                            //                % Reduce matrix.
                            //                [A,CH,RH]=hmreduce(A,CH,RH,LC,LR,SLC,SLR);
                            Hmreduce(A, ref CH, ref RH, LC, LR, SLC, SLR);
                            //            end
                        }

                        //            % Re-start with first unexplored row.
                        //            r=RH(n+1);
                        r = RH[n];
                        //            % Get column of next free zero in row r.
                        //            l=CH(r);
                        l = CH[r];
                        //            % Advance "column of next free zero".
                        //            CH(r)=-A(r,l);
                        CH[r] = -(int)A[r, l];
                        //            % If this zero is last in the list..
                        //            if (A(r,l)==0)
                        if (isNoMatch(A[r, l])) {
                            //                % ...remove row r from unexplored list.
                            //                RH(n+1)=RH(r);
                            RH[n] = RH[r];
                            //                RH(r)=0;
                            RH[r] = NoMatch;
                            //            end
                        }
                        //        end
                    }

                    //        % While the column l is labelled, i.e. in path.
                    //        while (LC(l)~=0)
                    while (isNotNoMatch(LC[l])) {
                        //            % If row r is explored..
                        //            if (RH(r)==0)
                        if (isNoMatch(RH[r])) {
                            //                % If all rows are explored..
                            //                if (RH(n+1)<=0)
                            if (RH[n] <= 0) { // TODO 不知道
                                //                    % Reduce cost matrix.
                                //                    [A,CH,RH]=hmreduce(A,CH,RH,LC,LR,SLC,SLR);
                                Hmreduce(A, ref CH, ref RH, LC, LR, SLC, SLR);
                                //                end
                            }

                            //                % Re-start with first unexplored row.
                            //                r=RH(n+1);
                            r = RH[n];
                            //            end
                        }

                        //            % Get column of next free zero in row r.
                        //            l=CH(r);
                        l = CH[r];

                        //            % Advance "column of next free zero".
                        //            CH(r)=-A(r,l);
                        CH[r] = -(int)A[r, l];

                        //            % If this zero is last in list..
                        //            if(A(r,l)==0)
                        if (isNoMatch(A[r, l])) {
                            //                % ...remove row r from unexplored list.
                            //                RH(n+1)=RH(r);
                            RH[n] = RH[r];
                            //                RH(r)=0;
                            RH[r] = NoMatch;
                            //            end
                        }
                        //        end
                    }

                    //        % If the column found is unassigned..
                    //        if (C(l)==0)
                    if (isNoMatch(C[l])) {
                        //            % Flip all zeros along the path in LR,LC.
                        //            [A,C,U]=hmflip(A,C,LC,LR,U,l,r);
                        Hmflip(A, ref C, ref U, LC, LR, l, r);
                        //            % ...and exit to continue with next unassigned row.
                        //            break;
                        break;
                        //        else
                    } else {
                        //            % ...else add zero to path.

                        //            % Label column l with row r.
                        //            LC(l)=r;
                        LC[l] = r;

                        //            % Add l to the set of labelled columns.
                        //            SLC=[SLC l];
                        SLC.Add(l);

                        //            % Continue with the row assigned to column l.
                        //            r=C(l);
                        r = C[l];

                        //            % Label row r with column l.
                        //            LR(r)=l;
                        LR[r] = l;

                        //            % Add r to the set of labelled rows.
                        //            SLR=[SLR r];
                        SLR.Add(r);
                        //        end
                    }
                    //    end
                }
                //end
            }

            //% Calculate the total cost.
            //T=sum(orig(logical(sparse(C,1:size(orig,2),1))));
            // TODO 不知道啥意思

            throw new NotImplementedException();
        }

        private Matrix Hminired(Matrix A) {
            //function A=hminired(A)
            //%HMINIRED Initial reduction of cost matrix for the Hungarian method.
            //%
            //%B=assredin(A)
            //%A - the unreduced cost matris.
            //%B - the reduced cost matrix with linked zeros in each row.

            //% v1.0  96-06-13. Niclas Borlin, niclas@cs.umu.se.

            //[m,n]=size(A);
            int m = A.Rows, n = A.Columns;

            //% Subtract column-minimum values from each column.
            //colMin=min(A);
            var colMin = new DenseVector(A.GetColumns().Select(col => col.Min()).ToArray());
            //A=A-colMin(ones(n,1),:);
            for (int i = 0; i < A.Rows; ++i) {
                A.SetRow(i, A.GetRow(i) - colMin);
            }

            //% Subtract row-minimum values from each row.
            //rowMin=min(A')';
            var rowMin = new DenseVector(A.GetRows().Select(row => row.Min()).ToArray());
            //A=A-rowMin(:,ones(1,n));
            for (int j = 0; j < A.Rows; ++j) {
                A.SetColumn(j, A.GetColumn(j) - rowMin);
            }

            //% Get positions of all zeros.
            //[i,j]=find(A==0);
            List<int> ilist = new List<int>();
            List<int> jlist = new List<int>();
            A.EachT((v, i, j) => {
                if (v == 0) {
                    ilist.Add(i);
                    jlist.Add(j);
                }
            });

            //% Extend A to give room for row zero list header column.
            //A(1,n+1)=0;
            Matrix tmp = Zeros(n, n + 1);
            tmp.SetSubMatrix(0, n, 0, n, A);
            //for k=1:n
            for (int k = 0; k < n; ++k) {
                //    % Get all column in this row. 
                //    cols=j(k==i)';
                var cols = new List<int>();
                cols.Add(n);
                for (int i = 0; i < ilist.Count; ++i) {
                    if (ilist[i] == k) {
                        cols.Add(jlist[i]);
                    }
                }
                cols.Add(-1);

                //    % Insert pointers in matrix.
                //    A(k,[n+1 cols])=[-cols 0];
                for (int i = 0; i < cols.Count - 1; ++i) {
                    tmp[k, cols[i]] = -(cols[i + 1]) - 1;
                } // TODO 不知道对不对了
                //result[k, cols[cols.Count - 1]] = 0;
                //end
            }
            var result = tmp.Each(v => {
                if (v < 0) return v + 1;
                else if (v == 0) return NoMatch;
                else return v;
            });

            return result;
        }

        private void Hminiass(Matrix A, out int[] C, out int[] U) {
            //function [A,C,U]=hminiass(A)
            //%HMINIASS Initial assignment of the Hungarian method.
            //%
            //%[B,C,U]=hminiass(A)
            //%A - the reduced cost matrix.
            //%B - the reduced cost matrix, with assigned zeros removed from lists.
            //%C - a vector. C(J)=I means row I is assigned to column J,
            //%              i.e. there is an assigned zero in position I,J.
            //%U - a vector with a linked list of unassigned rows.

            //% v1.0  96-06-14. Niclas Borlin, niclas@cs.umu.se.

            //[n,np1]=size(A);
            int n = A.Rows, np1 = A.Columns;

            //% Initalize return vectors.
            //C=zeros(1,n);
            C = Utils.InitArray<int>(n, NoMatch);
            //U=zeros(1,n+1);
            U = Utils.InitArray<int>(n + 1, NoMatch);

            //% Initialize last/next zero "pointers".
            //LZ=zeros(1,n);
            var LZ = Utils.InitArray<int>(n, NoMatch);
            //NZ=zeros(1,n);
            var NZ = Utils.InitArray<int>(n, NoMatch);

            //for i=1:n
            for (int i = 0; i < n; ++i) {
                //    % Set j to first unassigned zero in row i.
                //    lj=n+1;
                int lj = n;
                //    j=-A(i,lj);
                int j = -(int)A[i, lj];

                //    % Repeat until we have no more zeros (j==0) or we find a zero
                //    % in an unassigned column (c(j)==0).

                //    while (C(j)~=0)
                while (isNotNoMatch(j) && isNotNoMatch(C[j])) {
                    //        % Advance lj and j in zero list.
                    //        lj=j;
                    lj = j;
                    //        j=-A(i,lj);
                    j = -(int)A[i, lj];

                    //        % Stop if we hit end of list.
                    if (isNoMatch(j)) break;
                    //        if (j==0)
                    //            break;
                    //        end
                    //    end
                }

                //    if (j~=0)
                if (isNotNoMatch(j)) {
                    //        % We found a zero in an unassigned column.

                    //        % Assign row i to column j.
                    //        C(j)=i;
                    C[j] = i;

                    //        % Remove A(i,j) from unassigned zero list.
                    //        A(i,lj)=A(i,j);
                    A[i, lj] = A[i, j];

                    //        % Update next/last unassigned zero pointers.
                    //        NZ(i)=-A(i,j);
                    NZ[i] = -(int)A[i, j];
                    //        LZ(i)=lj;
                    LZ[i] = lj;

                    //        % Indicate A(i,j) is an assigned zero.
                    //        A(i,j)=0;
                    A[i, j] = NoMatch;
                } else {
                    //    else
                    //        % We found no zero in an unassigned column.

                    //        % Check all zeros in this row.

                    //        lj=n+1;
                    lj = n;
                    //        j=-A(i,lj);
                    j = -(int)A[i, lj];

                    //        % Check all zeros in this row for a suitable zero in another row.
                    //        while (j~=0)
                    while (isNotNoMatch(j)) {
                        //            % Check the in the row assigned to this column.
                        //            r=C(j);
                        int r = C[j];

                        //            % Pick up last/next pointers.
                        //            lm=LZ(r);
                        int lm = LZ[r];
                        //            m=NZ(r);
                        int m = NZ[r];

                        //            % Check all unchecked zeros in free list of this row.
                        //            while (m~=0)
                        while (isNotNoMatch(m)) {
                            //                % Stop if we find an unassigned column.
                            //                if (C(m)==0)
                            if (isNoMatch(C[m])) {
                                //                    break;
                                break;
                                //                end
                            }

                            //                % Advance one step in list.
                            //                lm=m;
                            lm = m;
                            //                m=-A(r,lm);
                            m = -(int)A[r, lm];
                            //            end
                        }

                        //            if (m==0)
                        if (isNoMatch(m)) {
                            //                % We failed on row r. Continue with next zero on row i.
                            //                lj=j;
                            lj = j;
                            //                j=-A(i,lj);
                            j = -(int)A[i, lj];
                            //            else
                        } else {

                            //                % We found a zero in an unassigned column.

                            //                % Replace zero at (r,m) in unassigned list with zero at (r,j)
                            //                A(r,lm)=-j;
                            A[r, lm] = -j;
                            //                A(r,j)=A(r,m);
                            A[r, j] = A[r, m];

                            //                % Update last/next pointers in row r.
                            //                NZ(r)=-A(r,m);
                            NZ[r] = -(int)A[r, m];
                            //                LZ(r)=j;
                            LZ[r] = j;

                            //                % Mark A(r,m) as an assigned zero in the matrix . . .
                            //                A(r,m)=0;
                            A[r, m] = NoMatch;

                            //                % ...and in the assignment vector.
                            //                C(m)=r;
                            C[m] = r;

                            //                % Remove A(i,j) from unassigned list.
                            //                A(i,lj)=A(i,j);
                            A[i, lj] = A[i, j];

                            //                % Update last/next pointers in row r.
                            //                NZ(i)=-A(i,j);
                            NZ[i] = -(int)A[i, j];
                            //                LZ(i)=lj;
                            LZ[i] = lj;

                            //                % Mark A(r,m) as an assigned zero in the matrix . . .
                            //                A(i,j)=0;
                            A[i, j] = NoMatch;

                            //                % ...and in the assignment vector.
                            //                C(j)=i;
                            C[j] = i;

                            //                % Stop search.
                            //                break;
                            break;
                            //            end
                        }
                        //        end
                    }
                    //    end
                }
                //end
            }

            //% Create vector with list of unassigned rows.

            //% Mark all rows have assignment.
            //r=zeros(1,n);
            int[] rr = Utils.InitArray<int>(n, NoMatch);
            //rows=C(C~=0);
            //r(rows)=rows;
            for (int i = 0; i < n; ++i) {
                int r = C[i];
                if (isNotNoMatch(r)) rr[r] = r;
            }
            //empty=find(r==0);
            var empty = new List<int>();
            empty.Add(n);
            empty.AddRange(rr.Select((r, i) => new { R = r, Idx = i }).Where(r => isNoMatch(r.R)).Select(r => r.Idx));
            empty.Add(NoMatch);
            //% Create vector with linked list of unassigned rows.
            //U=zeros(1,n+1);
            U = Utils.InitArray<int>(n + 1, NoMatch);
            //U([n+1 empty])=[empty 0];
            for (int i = 0; i < empty.Count - 1; ++i) {
                U[empty[i]] = empty[i + 1];
            }
        }

        private void Hmflip(Matrix A, ref int[] C, ref int[] U, int[] LC, int[] LR, int l, int r) {
            //function [A,C,U]=hmflip(A,C,LC,LR,U,l,r)
            //%HMFLIP Flip assignment state of all zeros along a path.
            //%
            //%[A,C,U]=hmflip(A,C,LC,LR,U,l,r)
            //%Input:
            //%A   - the cost matrix.
            //%C   - the assignment vector.
            //%LC  - the column label vector.
            //%LR  - the row label vector.
            //%U   - the 
            //%r,l - position of last zero in path.
            //%Output:
            //%A   - updated cost matrix.
            //%C   - updated assignment vector.
            //%U   - updated unassigned row list vector.

            //% v1.0  96-06-14. Niclas Borlin, niclas@cs.umu.se.

            //n=size(A,1);
            int n = A.Rows;

            //while (1)
            while (true) {
                //    % Move assignment in column l to row r.
                //    C(l)=r;
                C[l] = r;

                //    % Find zero to be removed from zero list..

                //    % Find zero before this.
                //    m=find(A(r,:)==-l);
                int[] m = A.GetRow(r).FindIdxBy(v => v == -l);

                //    % Link past this zero.
                //    A(r,m)=A(r,l);
                for (int i = 0; i < m.Length; ++i) {
                    A[r, m[i]] = A[r, l];
                }

                //    A(r,l)=0;
                A[r, l] = NoMatch;

                //    % If this was the first zero of the path..
                //    if (LR(r)<0)
                if (LR[r] <= 0 && isNotNoMatch(LR[r])) { // TODO <=0不确定
                    //        ...remove row from unassigned row list and return.
                    //        U(n+1)=U(r);
                    U[n] = U[r];
                    //        U(r)=0;
                    U[r] = NoMatch;
                    //        return;
                    return;
                    //    else
                } else {

                    //        % Move back in this row along the path and get column of next zero.
                    //        l=LR(r);
                    l = LR[r];

                    //        % Insert zero at (r,l) first in zero list.
                    //        A(r,l)=A(r,n+1);
                    A[r, l] = A[r, n];
                    //        A(r,n+1)=-l;
                    A[r, n] = -l;

                    //        % Continue back along the column to get row of next zero in path.
                    //        r=LC(l);
                    r = LC[l];
                    //    end
                }
                //end
            }
        }

        private void Hmreduce(Matrix A, ref int[] CH, ref int[] RH, int[] LC, int[] LR, List<int> SLC, List<int> SLR) {
            //function [A,CH,RH]=hmreduce(A,CH,RH,LC,LR,SLC,SLR)
            //%HMREDUCE Reduce parts of cost matrix in the Hungerian method.
            //%
            //%[A,CH,RH]=hmreduce(A,CH,RH,LC,LR,SLC,SLR)
            //%Input:
            //%A   - Cost matrix.
            //%CH  - vector of column of 'next zeros' in each row.
            //%RH  - vector with list of unexplored rows.
            //%LC  - column labels.
            //%RC  - row labels.
            //%SLC - set of column labels.
            //%SLR - set of row labels.
            //%
            //%Output:
            //%A   - Reduced cost matrix.
            //%CH  - Updated vector of 'next zeros' in each row.
            //%RH  - Updated vector of unexplored rows.

            //% v1.0  96-06-14. Niclas Borlin, niclas@cs.umu.se.

            //n=size(A,1);
            int n = A.Rows;

            //% Find which rows are covered, i.e. unlabelled.
            //coveredRows=LR==0;
            var coveredRows = LR.Select(v => isNoMatch(v)).ToArray();

            //% Find which columns are covered, i.e. labelled.
            //coveredCols=LC~=0;
            var coveredCols = LC.Select(v => isNotNoMatch(v)).ToArray();

            //r=find(~coveredRows);
            var r = coveredRows.Select((v, i) => new { Val = v, Idx = i })
                               .Where(v => !v.Val)
                               .Select(v => v.Idx)
                               .ToArray();
            //c=find(~coveredCols);
            var c = coveredCols.Select((v, i) => new { Val = v, Idx = i })
                               .Where(v => !v.Val)
                               .Select(v => v.Idx)
                               .ToArray();

            //% Get minimum of uncovered elements.
            //m=min(min(A(r,c)));
            double m = double.MaxValue;
            foreach (int i in r) {
                foreach (int j in c) {
                    var val = A[i, j];
                    if (val < m) m = val;
                }
            }

            //% Subtract minimum from all uncovered elements.
            //A(r,c)=A(r,c)-m;
            foreach (int i in r) {
                foreach (int j in c) {
                    A[i, j] -= m;
                }
            }

            //% Check all uncovered columns..
            //for j=c
            foreach (int j in c) {
                //    % ...and uncovered rows in path order..
                //    for i=SLR
                foreach (int i in SLR) {
                    //        % If this is a (new) zero..
                    //        if (A(i,j)==0)
                    if (isNoMatch(A[i, j])) {
                        //            % If the row is not in unexplored list..
                        //            if (RH(i)==0)
                        if (isNoMatch(RH[i])) {
                            //                % ...insert it first in unexplored list.
                            //                RH(i)=RH(n+1);
                            RH[i] = RH[n];
                            //                RH(n+1)=i;
                            RH[n] = i;
                            //                % Mark this zero as "next free" in this row.
                            //                CH(i)=j;
                            CH[i] = j;
                            //            end
                        }
                        //            % Find last unassigned zero on row I.
                        //            row=A(i,:);
                        var row = A.GetRow(i);
                        //            colsInList=-row(row<0);
                        var colsInList = row.Where(v => v <= 0 && isNotNoMatch(v)).Select(v => -v).ToArray(); // TODO <=0不确定
                        int l = NoMatch;
                        //            if (length(colsInList)==0)
                        if (colsInList.Length == 0) {
                            //                % No zeros in the list.
                            //                l=n+1;
                            l = n;
                        } else {
                            //            else
                            //                l=colsInList(row(colsInList)==0);
                            /*
                             * row=[2 -5 0 -3 6 -1];
                             * cil=-row(row<0);     5   3   1
                             * row(cil)==0;         0   1   0
                             * cil(row(cil)==0);    3
                             * */
                            foreach (int col in colsInList) {
                                if (isNoMatch(row[col])) {
                                    l = col;
                                    A[i, l] = -j; // TODO:这样不知道对不对
                                }
                            }
                            //            end
                        }
                        //            % Append this zero to end of list.
                        //            A(i,l)=-j;
                        A[i, l] = -j;
                        //        end
                    }
                    //    end
                }
                //end
            }

            //% Add minimum to all doubly covered elements.
            //r=find(coveredRows);
            r = coveredRows.Select((v, i) => new { Val = v, Idx = i })
                           .Where(v => v.Val)
                           .Select(v => v.Idx)
                           .ToArray();
            //c=find(coveredCols);
            c = coveredCols.Select((v, i) => new { Val = v, Idx = i })
                           .Where(v => v.Val)
                           .Select(v => v.Idx)
                           .ToArray();

            //% Take care of the zeros we will remove.
            //[i,j]=find(A(r,c)<=0);
            var ilist = new List<int>();
            var jlist = new List<int>();
            foreach (int i in r) {
                foreach (int j in c) {
                    if (A[i, j] <= 0) { // TODO <=0不知道对不对
                        ilist.Add(i);
                        jlist.Add(j);
                    }
                }
            }

            //i=r(i);
            //j=c(j);
            // TODO 不知道干嘛，似乎我上面的循环已经包含了这个功能

            //for k=1:length(i)
            for (int k = 0; k < ilist.Count; ++k) {
                //    % Find zero before this in this row.
                //    lj=find(A(i(k),:)==-j(k));
                var ljlist = A.GetRow(ilist[k]).FindIdxBy(d => d != -jlist[k]);
                //    % Link past it.
                //    A(i(k),lj)=A(i(k),j(k));
                foreach (var lj in ljlist) {
                    A[ilist[k], lj] = A[ilist[k], jlist[k]];
                }
                //    % Mark it as assigned.
                //    A(i(k),j(k))=0;
                A[ilist[k], jlist[k]] = NoMatch;
                //end
            }

            //A(r,c)=A(r,c)+m;
            foreach (int i in r) {
                foreach (int j in c) {
                    A[i, j] += m;
                }
            }
        }
    }
}
