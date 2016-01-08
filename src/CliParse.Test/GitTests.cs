using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CliParse.Tests.GitExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GitTests
    {
        [TestMethod]
        public void can_mimic_git_command_line()
        {
            Dictionary<string, GitParsable> commands = new Dictionary<string, GitParsable>()
            {
                {"add", new GitParsable(){GitCommand = GitCommand.add}},
                {"add <filename>", new GitParsable(){GitCommand = GitCommand.add, LastParam = "<filename>"}},
                {"add *", new GitParsable(){GitCommand = GitCommand.add, LastParam = "*"}},

                {"branch", new GitParsable(){GitCommand = GitCommand.branch}},
                {"branch -d <branchname>", new GitParsable(){GitCommand = GitCommand.branch, DeleteFlag = true, LastParam = "<branchname>"}},

                {"checkout", new GitParsable(){GitCommand = GitCommand.checkout}},
                {"checkout -b <branchname>", new GitParsable(){GitCommand = GitCommand.checkout, CreateBranch = true, LastParam = "<branchname>"}},
                {"checkout branch", new GitParsable(){GitCommand = GitCommand.checkout, LastParam = "branch"}},
                {"checkout -- <filename>", new GitParsable(){GitCommand = GitCommand.checkout, SecondLastParam = "--", LastParam = "<filename>"}},

                {"clone", new GitParsable(){GitCommand = GitCommand.clone}},
                {"clone /path/to/repository", new GitParsable(){GitCommand = GitCommand.clone, LastParam = "/path/to/repository"}},
                {"clone username@host:/path/to/repository", new GitParsable(){GitCommand = GitCommand.clone, LastParam = "username@host:/path/to/repository"}},

                {"commit", new GitParsable(){GitCommand = GitCommand.commit}},
                {"commit -m \"commit message\"", new GitParsable(){GitCommand = GitCommand.commit, Message="commit message"}},
                {"commit -a", new GitParsable(){GitCommand = GitCommand.commit, AddFlag = true}},
                {"commit -am \"commit message\"", new GitParsable(){GitCommand = GitCommand.commit, AddFlag = true, Message="commit message"}},

                {"config", new GitParsable(){GitCommand = GitCommand.config}},
                {"config --global user.name \"Sam Smith\"", new GitParsable(){GitCommand = GitCommand.config, GlobalConfigName = "user.name", LastParam = "Sam Smith"}},
                {"config --global user.email sam@example.com", new GitParsable(){GitCommand = GitCommand.config, GlobalConfigName = "user.email", LastParam = "sam@example.com"}},

                {"diff", new GitParsable(){GitCommand = GitCommand.diff}},
                {"diff --base <filename>", new GitParsable(){GitCommand = GitCommand.diff, BaseFlag=true, LastParam = "<filename>"}},
                {"diff <sourcebranch> <targetbranch>", new GitParsable(){GitCommand = GitCommand.diff, SecondLastParam = "<sourcebranch>", LastParam = "<targetbranch>"}},

                {"fetch", new GitParsable(){GitCommand = GitCommand.fetch}},
                {"fetch origin", new GitParsable(){GitCommand = GitCommand.fetch, LastParam = "origin"}},

                {"grep", new GitParsable(){GitCommand = GitCommand.grep}},
                {"grep \"foo()\"", new GitParsable(){GitCommand = GitCommand.grep, LastParam = "foo()"}},

                {"init", new GitParsable(){GitCommand = GitCommand.init}},
                {"log", new GitParsable(){GitCommand = GitCommand.log}},
                
                {"merge", new GitParsable(){GitCommand = GitCommand.merge}},
                {"merge <branchname>", new GitParsable(){GitCommand = GitCommand.merge, LastParam = "<branchname>"}},

                {"pull", new GitParsable(){GitCommand = GitCommand.pull}},
                
                {"push", new GitParsable(){GitCommand = GitCommand.push}},
                {"push origin master", new GitParsable(){GitCommand = GitCommand.push, LastParam = "master", SecondLastParam="origin"}},
                {"push origin <branchname>", new GitParsable(){GitCommand = GitCommand.push, LastParam = "<branchname>", SecondLastParam="origin"}},
                {"push --all origin", new GitParsable(){GitCommand = GitCommand.push, AllFlag = true, LastParam = "origin"}},
                {"push origin :<branchname>", new GitParsable(){GitCommand = GitCommand.push, SecondLastParam = "origin", LastParam = ":<branchname>"}},
                {"push --tags origin", new GitParsable(){GitCommand = GitCommand.push, TagsFlag =true, LastParam = "origin"}},

                {"remote", new GitParsable(){GitCommand = GitCommand.remote}},
                {"remote add origin <server>", new GitParsable(){GitCommand = GitCommand.remote, LastParam = "<server>", SecondLastParam = "origin", ThirdLastParam = "add"}},
                {"remote -v", new GitParsable(){GitCommand = GitCommand.remote, Verbose = true}},

                {"reset", new GitParsable(){GitCommand = GitCommand.reset}},
                {"reset --hard origin/master", new GitParsable(){GitCommand = GitCommand.reset, HardFlag=true,LastParam = "origin/master"}},

                {"status", new GitParsable(){GitCommand = GitCommand.status}},

                {"tag", new GitParsable(){GitCommand = GitCommand.tag}},
                {"tag 1.0.0 <commitID>", new GitParsable(){GitCommand = GitCommand.tag, SecondLastParam = "1.0.0", LastParam = "<commitID>"}},
            };

            foreach (var command in commands.Keys)
            {
                var args = NativeMethods.CommandLineToArgs(command);
                var actual = new GitParsable();
                var result = actual.CliParse(args);

                Assert.IsTrue(result.Successful, command);
                Assert.IsFalse(result.ShowHelp, command);
                Assert.AreEqual(0, result.CliParseMessages.Count(), command);

                var expected = commands[command];
                Assert.AreEqual(expected.GitCommand, actual.GitCommand,
                    String.Format("Command '{0}' resulted in an incorrect GitCommand value of '{1}'", command,
                        actual.GitCommand));

                Assert.AreEqual(expected.GlobalConfigName, actual.GlobalConfigName,
                    String.Format("Command '{0}' resulted in an incorrect GlobalConfigName value of '{1}'", command,
                        actual.GlobalConfigName));

                Assert.AreEqual(expected.LastParam, actual.LastParam,
                    String.Format("Command '{0}' resulted in an incorrect LastParam value of '{1}'", command,
                        actual.LastParam));

                Assert.AreEqual(expected.SecondLastParam, actual.SecondLastParam,
                    String.Format("Command '{0}' resulted in an incorrect SecondLastParam value of '{1}'", command,
                        actual.SecondLastParam));

                Assert.AreEqual(expected.ThirdLastParam, actual.ThirdLastParam,
                    String.Format("Command '{0}' resulted in an incorrect ThirdLastParam value of '{1}'", command,
                        actual.ThirdLastParam));

                Assert.AreEqual(expected.Message, actual.Message,
                    String.Format("Command '{0}' resulted in an incorrect Message value of '{1}'", command,
                        actual.Message));

                Assert.AreEqual(expected.AddFlag, actual.AddFlag,
                    String.Format("Command '{0}' resulted in an incorrect AddFlag value of '{1}'", command,
                        actual.AddFlag));

                Assert.AreEqual(expected.CreateBranch, actual.CreateBranch,
                    String.Format("Command '{0}' resulted in an incorrect CreateBranch value of '{1}'", command,
                        actual.CreateBranch));

                Assert.AreEqual(expected.DeleteFlag, actual.DeleteFlag,
                    String.Format("Command '{0}' resulted in an incorrect DeleteFlag value of '{1}'", command,
                        actual.DeleteFlag));

                Assert.AreEqual(expected.TagsFlag, actual.TagsFlag,
                    String.Format("Command '{0}' resulted in an incorrect TagsFlag value of '{1}'", command,
                        actual.TagsFlag));

                Assert.AreEqual(expected.HardFlag, actual.HardFlag,
                    String.Format("Command '{0}' resulted in an incorrect HardFlag value of '{1}'", command,
                        actual.HardFlag));
            }
        }

        [TestMethod]
        public void can_mimic_git_command_line_specific()
        {
            Dictionary<string, GitParsable> commands = new Dictionary<string, GitParsable>()
            {
                {"commit -am \"commit message\"", new GitParsable(){GitCommand = GitCommand.commit, AddFlag = true, Message="commit message"}}
            };

            foreach (var command in commands.Keys)
            {
                var args = NativeMethods.CommandLineToArgs(command);
                var actual = new GitParsable();
                var result = actual.CliParse(args);

                Assert.IsTrue(result.Successful, command);
                Assert.IsFalse(result.ShowHelp, command);
                Assert.AreEqual(0, result.CliParseMessages.Count(), command);

                var expected = commands[command];
                Assert.AreEqual(expected.GitCommand, actual.GitCommand,
                    String.Format("Command '{0}' resulted in an incorrect GitCommand value of '{1}'", command,
                        actual.GitCommand));

                Assert.AreEqual(expected.GlobalConfigName, actual.GlobalConfigName,
                    String.Format("Command '{0}' resulted in an incorrect GlobalConfigName value of '{1}'", command,
                        actual.GlobalConfigName));

                Assert.AreEqual(expected.LastParam, actual.LastParam,
                    String.Format("Command '{0}' resulted in an incorrect LastParam value of '{1}'", command,
                        actual.LastParam));

                Assert.AreEqual(expected.Message, actual.Message,
                    String.Format("Command '{0}' resulted in an incorrect Message value of '{1}'", command,
                        actual.Message));
            }
        }
    }
}
