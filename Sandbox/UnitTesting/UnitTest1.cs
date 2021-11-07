using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steadsoft.Novus.Parser;
using Steadsoft.Novus.Scanner;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string TEXT =
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

            var parser = Parser.CreateParser(SourceText.Text, TEXT, TokenDefinition.Pathname, "novus.csv");

            parser.TryParse(out var root);

            Assert.IsTrue(root.Children.Count == 7);

            Assert.IsTrue(root.Children[0] is NamespaceStatement);
            Assert.IsTrue(((NamespaceStatement)(root.Children[0])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[1] is NamespaceStatement);
            Assert.IsTrue(((NamespaceStatement)(root.Children[1])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[2] is NamespaceStatement);
            Assert.IsTrue(root.Children[3] is NamespaceStatement);
            Assert.IsTrue(root.Children[4] is TypeStatement);
            Assert.IsTrue(root.Children[5] is TypeStatement);

            Assert.IsTrue(root.Children[6] is TypeStatement);
            Assert.IsTrue(((TypeStatement)(root.Children[6])).Body.Children.Count == 1);

            ;
        }
    }
}