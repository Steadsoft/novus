namespace UnitTesting
{
    public static class Source
    {
        public const string ValidText_1 =
            @"
            namespace A
            {
                namespace b
                {
                    namespace namespace
                    {
                        type Category class sealed sealed abstract static public private
                        {
                            type Window class public abstract sealed
                            {

                            }
                        }
                    }
                } 
            }

            namespace d
            {
                type Umbrealla class
                {

                }
            }

            namespace abc;

            // using Steadsoft.Novus.Support;

            namespace Steadsoft.Novus.Support
            {
                namespace Steadsoft.Novus.Compiler
                {

                }
            }

            type Umbrealla class
            {

            }

            type Window class public abstract sealed
            {

            }

            type Category1 class sealed sealed abstract static
            {
                type Category2 class sealed sealed abstract static
                {
                    type Category3 class sealed sealed abstract static
                    {
                        type Category4 class sealed sealed abstract static
                        {

                        }
                    }
                }
            }
            ";
        public const string DelimiterTest_1 =
            @"
            #demark |||

            |||lkfhjsdkfhsdkfghsdkgfhdfkg|||

            ""jhdkjhdkajshdkajshdkkhsdhd""

            ";

    }
}