using System;
using System.Text;
using System.Numerics;

namespace BigIntegerMath {

    public class MathB {

        public MathB() {
        }

        /// <summary>
        /// xとyの最大公約数
        /// </summary>
        public BigInteger GCD(BigInteger x,BigInteger y) {
            if(x < y)
                SwapNum(ref x,ref y);

            return y == 0 ? x : GCD(y,x % y);
        }

        /// <summary>
        /// xとyの最小公倍数
        /// </summary>
        public BigInteger LCM(BigInteger x,BigInteger y) {
            return x * y / GCD(x,y);
        }


        /// <summary>
        /// BigIntegerの二数を入れ替える
        /// </summary>
        public void SwapNum(ref BigInteger x,ref BigInteger y) {
            BigInteger tmp;
            tmp = x;
            x = y;
            y = tmp;
        }

        /// <summary>
        /// 一次不定方程式の特殊解
        /// </summary>
        public (BigInteger x, BigInteger y) SpecialLIE(BigInteger a,BigInteger b) {
            BigInteger x = 0, y = 0;

            _LIE(a,b,ref x,ref y);

            return (x, y);
        }
        /// <summary>
        /// 拡張ユークリッド
        /// </summary>
        private BigInteger _LIE(BigInteger a,BigInteger b,ref BigInteger x,ref BigInteger y) {
            if(b == 0) {
                x = 1;
                y = 0;
                return a;
            }

            BigInteger d = _LIE(b,a % b,ref y,ref x);

            y -= a / b * x;

            return d;
        }

        /// <summary>
        /// 拡張ユークリッド
        /// </summary>
        private (BigInteger, BigInteger, BigInteger) ExtendedEuclid(BigInteger x,BigInteger y) {
            var (c0, c1) = (x, y);
            (BigInteger a0, BigInteger a1) = (1, 0);
            (BigInteger b0, BigInteger b1) = (0, 1);
            BigInteger m, q;
            BigInteger r = 0;

            while(c1 != 0) {
                m = c0 % c1;
                q = BigInteger.DivRem(c0,c1,out r);

                (c0, c1) = (c1, m);
                (a0, a1) = (a1, (a0 - q * a1));
                (b0, b1) = (b1, (b0 - q * b1));
            }
            return (c0, a0, b0);
        }

        /// <summary>
        /// 一次不定一般解の一般解を(x=?+?t,y=?+?t)という二つのstring型で返す
        /// </summary>
        public (string x, string y) GeneralLIE(BigInteger a,BigInteger b) {
            BigInteger c = GCD(a,b);

            var (x, y) = SpecialLIE(a,b);

            return ($"x={x}+{b / c}t", $"y={y}-{a / c}t");
        }

        /// <summary>
        /// 一次合同式ax≡b(mod m)のxを求める
        /// 解がない場合は-1を返す
        /// </summary>
        public BigInteger Congruence(BigInteger a,BigInteger b,BigInteger m) {
            if(m <= 0)
                return -1;

            for(int i = 0 ; i < m ; i++) {
                if((a * i - b) % m == 0) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 素数判定
        /// </summary>
        public bool CheckPrime(BigInteger n) {
            if(n == 2)
                return true;
            if(n <= 1 || CheckEven(n))
                return false;

            var d = (n - 1) >> 1;
            while(CheckEven(d))
                d >>= 1;

            for(int i = 0 ; i < 100 ; i++) {
                var a = RandomNext(1,n - 1);
                var t = d;
                var y = BigInteger.ModPow(a,t,n);

                while(t != n - 1 && y != 1 && y != n - 1) {
                    y = (y * y) % n;
                    t <<= 1;
                }

                if(y != n - 1 && CheckEven(t))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 指定された桁数の素数を返す
        /// </summary>
        public BigInteger GetPrime(short digit) {
            BigInteger rand;

            while(true) {
                rand = RandomDigit(digit);
                if(CheckEven(rand))
                    rand += 1;
                if(CheckPrime(rand))
                    break;
            }
            return rand;
        }

        /// <summary>
        /// 偶数判定
        /// </summary>
        public bool CheckEven(BigInteger n) {
            return n % 2 == 0;
        }

        /// <summary>
        /// 指定した桁数の乱数を返す
        /// </summary>
        /// <param name="digit">桁数</param>
        public BigInteger RandomDigit(int digit) {
            Random rand = new Random();

            StringBuilder numbers = new StringBuilder();

            for(int n = 0 ; n < digit ; n++) {
                numbers.Append(rand.Next(0,10).ToString());
            }

            BigInteger val = BigInteger.Parse(numbers.ToString());

            return val;
        }

        /// <summary>
        /// minからmaxのランダムな値を返す
        /// minとmaxが同じならmaxを返す
        /// </summary>
        public BigInteger RandomNext(BigInteger min,BigInteger max) {
            if(min == max)
                return max;

            Random rand = new Random();
            BigInteger r = 0;
            var digit = rand.Next(GetDigit(min),GetDigit(max) + 1);

            while(true) {
                r = RandomDigit(digit);
                if(min <= r && max >= r)
                    break;
            }
            return r;
        }

        /// <summary>
        /// 桁数を返す
        /// </summary>
        public int GetDigit(BigInteger n) {
            return (n == 0) ? 1 : ((int)BigInteger.Log10(n) + 1);
        }
    }
}

