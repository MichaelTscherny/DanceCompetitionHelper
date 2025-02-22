using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.Helper;

using MigraDoc.DocumentObjectModel;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.Helper
{
    [TestFixture]
    public class SimplePdfTableLayouterTests
    {
        public static readonly object[][] DoLayout_TestData = new object[][]
        {
            // ------
            // ------
            new object[]
            {
                "% 50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    100.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(100),
                }
            },
            // ------
            new object[]
            {
                "% 50/50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            new object[]
            {
                "% 50/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            new object[]
            {
                "% */50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            // ------
            new object[]
            {
                "% 33/66",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    33.0,
                    66.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    33.0,
                    66.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(33),
                    Unit.FromMillimeter(66),
                }
            },
            new object[]
            {
                "% 33/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    33.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    33.0,
                    67.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(33),
                    Unit.FromMillimeter(67),
                }
            },
            new object[]
            {
                "% */66",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    66.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    34.0,
                    66.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(34),
                    Unit.FromMillimeter(66),
                }
            },
            // ------
            new object[]
            {
                "% 25/25/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 25/*/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    0.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 25/25/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    0.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 25/25/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 25/*/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    0.0,
                    0.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 25/*/*/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    25.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            // ------
            new object[]
            {
                "% 50/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 50/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                    0.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% 50/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "% */25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            // ------
            new object[]
            {
                "% 10/15/*/33",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    10.0,
                    15.0,
                    0.0,
                    33.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    10.0,
                    15.0,
                    42.0,
                    33.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(10),
                    Unit.FromMillimeter(15),
                    Unit.FromMillimeter(42),
                    Unit.FromMillimeter(33),
                }
            },
            // ------
            // ------
            new object[]
            {
                "mm 50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                },
                // 
                new double[]
                {
                    100.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(100),
                }
            },
            // ------
            new object[]
            {
                "mm 50/50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            new object[]
            {
                "mm 50/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            new object[]
            {
                "mm */50",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.FromMillimeter(50),
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(50),
                }
            },
            // ------
            new object[]
            {
                "mm 50/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 50/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 50/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm */25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    50.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            // ------
            // ------
            new object[]
            {
                "mm 25/25/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm */25/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 25/*/25/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 25/25/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 25/25/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 25/*/*/25",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm 25/*/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            new object[]
            {
                "mm */25/25/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                    Unit.FromMillimeter(25),
                }
            },
            // ------
            // ------
            new object[]
            {
                "% *",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    100.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(100),
                }
            },
            new object[]
            {
                "% */*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    50.0,
                    50.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(50.0),
                    Unit.FromMillimeter(50.0),
                }
            },
            new object[]
            {
                "% */*/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    33.33,
                    33.33,
                    33.34,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(33.33),
                    Unit.FromMillimeter(33.33),
                    Unit.FromMillimeter(33.34),
                }
            },
            new object[]
            {
                "% */*/*/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    25.0,
                    25.0,
                    25.0,
                    25.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(25.0),
                    Unit.FromMillimeter(25.0),
                    Unit.FromMillimeter(25.0),
                    Unit.FromMillimeter(25.0),
                }
            },
            new object[]
            {
                "% */*/*/*/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    20.0,
                    20.0,
                    20.0,
                    20.0,
                    20.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20.0),
                    Unit.FromMillimeter(20.0),
                    Unit.FromMillimeter(20.0),
                    Unit.FromMillimeter(20.0),
                    Unit.FromMillimeter(20.0),
                }
            },
            // ------
            // ------
            new object[]
            {
                "mixed 20mm/*/10%/10%",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    10.0,
                    10.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    20.0,
                    60.0,
                    10.0,
                    10.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.FromMillimeter(60),
                    Unit.FromMillimeter(10),
                    Unit.FromMillimeter(10),
                }
            },
            new object[]
            {
                "mixed 20mm/15mm/*/20%",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    20.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.FromMillimeter(15),
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    20.0,
                    15.0,
                    45.0,
                    20.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.FromMillimeter(15),
                    Unit.FromMillimeter(45),
                    Unit.FromMillimeter(20),
                }
            },
            new object[]
            {
                "mixed 20mm/15mm/*/*",
                Unit.FromMillimeter(100),
                // 
                new double[]
                {
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.FromMillimeter(15),
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    20.0,
                    15.0,
                    32.5,
                    32.5,
                },
                new Unit[]
                {
                    Unit.FromMillimeter(20),
                    Unit.FromMillimeter(15),
                    Unit.FromMillimeter(32.5),
                    Unit.FromMillimeter(32.5),
                }
            },
            // ------
            // ------
            new object[]
            {
                "prod 6cm/*/*/*/*/*/*/*/* (8x *)",
                Unit.FromMillimeter(247),
                // 
                new double[]
                {
                    0.0,
                    //
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                    //
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                },
                new Unit[]
                {
                    Unit.FromCentimeter(6),
                    //
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    //
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                    Unit.Empty,
                },
                // 
                new double[]
                {
                    24.00,
                    //
                    9.5,
                    9.5,
                    9.5,
                    9.5,
                    //
                    9.5,
                    9.5,
                    9.5,
                    9.5,
                },
                new Unit[]
                {
                    Unit.FromCentimeter(6),
                    //
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(23.47),
                    //
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(23.47),
                    Unit.FromMillimeter(22.71),
                }
            },
        };

        [Test]
        [TestCaseSource(nameof(DoLayout_TestData))]
        public void DoLayout_Test(
            string name,
            Unit useTableWidth,
            //
            double[] usePercentages,
            Unit[] useTableWidths,
            //
            double[] expectedPercentages,
            Unit[] expectedColumnWidths)
        {
            var expectedLength = usePercentages.Length;

            Assert.Multiple(() =>
            {
                Assert.That(
                    useTableWidths,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: useTableWidths.Length");
                Assert.That(
                    expectedPercentages,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: expectedPercentages.Length");
                Assert.That(
                    expectedColumnWidths,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: expectedColumnWidths.Length");
            });

            var test = new SimplePdfTableLayouter(
                useTableWidth);

            for (var idx = 0; idx < expectedLength; idx++)
            {
                var usePercentage = usePercentages[idx];
                var useWidth = useTableWidths[idx];

                if (usePercentage > 0.0)
                {
                    test.AddColumn(
                        usePercentage);
                    continue;
                }

                if (useWidth.IsZeroOrEmpty() == false)
                {
                    test.AddColumn(
                        useWidth);
                    continue;
                }

                test.AddColumn(
                    Unit.Empty);
            }

            test.DoLayout();

            Assert.Multiple(() =>
            {
                Assert.That(
                    test.ColumnWidthsPercent,
                    Has.Count.EqualTo(
                        expectedLength),
                    $"[{name}]: test.ColumnWidthsPercent");
                Assert.That(
                    test.ColumnWidthsPercent,
                    Is
                        .EqualTo(
                            expectedPercentages)
                        .AsCollection,
                    $"[{name}]: test.ColumnWidthsPercent Values");

                Assert.That(
                    test.ColumnWidths,
                    Has.Count.EqualTo(
                        expectedLength),
                    $"[{name}]: test.ColumnWidths");
                Assert.That(
                    test.ColumnWidths,
                    Is
                        .EqualTo(
                            expectedColumnWidths)
                        .AsCollection,
                    $"[{name}]: test.ColumnWidths Values");
            });
        }

        [Test]
        [TestCaseSource(nameof(DoLayout_TestData))]
        public void ApplyTo_Test(
            string name,
            Unit useTableWidth,
            //
            double[] usePercentages,
            Unit[] useTableWidths,
            //
            double[] expectedPercentages,
            Unit[] expectedColumnWidths)
        {
            var expectedLength = usePercentages.Length;

            Assert.Multiple(() =>
            {
                Assert.That(
                    useTableWidths,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: useTableWidths.Length");
                Assert.That(
                    expectedPercentages,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: expectedPercentages.Length");
                Assert.That(
                    expectedColumnWidths,
                    Has.Length.EqualTo(
                        expectedLength),
                    $"[{name}]: expectedColumnWidths.Length");
            });

            var test = new SimplePdfTableLayouter(
                useTableWidth);
            var testTable = new MigraDoc.DocumentObjectModel.Tables.Table
            {
                Borders =
                {
                    Width = 0.75,
                    Visible = true,
                },
            };

            for (var idx = 0; idx < expectedLength; idx++)
            {
                var usePercentage = usePercentages[idx];
                var useWidth = useTableWidths[idx];

                if (usePercentage > 0.0)
                {
                    test.AddColumn(
                        usePercentage);
                    continue;
                }

                if (useWidth.IsZeroOrEmpty() == false)
                {
                    test.AddColumn(
                        useWidth);
                    continue;
                }

                test.AddColumn(
                    Unit.Empty);
            }

            test.ApplyTo(
                testTable);

            Assert.Multiple(() =>
            {
                Assert.That(
                    testTable.Columns,
                    Has.Count.EqualTo(
                        expectedLength),
                    $"[{name}]: testTable.Columns");

                for (var idx = 0; idx < expectedLength; idx++)
                {
                    Assert.That(
                        testTable.Columns[idx].Width,
                        Is.EqualTo(
                            expectedColumnWidths[idx]),
                        $"[{name}]: testTable.Column[{idx}]");
                }
            });
        }
    }
}
