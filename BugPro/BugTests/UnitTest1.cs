using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void InitialStateIsNewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.NewDefect, bug.GetCurrentState());
    }

    [TestMethod]
    public void AssignToTeamMovesToAnalysis()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        Assert.AreEqual(Bug.State.Analysis, bug.GetCurrentState());
    }

    [TestMethod]
    public void InvestigateFromAnalysisGoesToFixed()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        Assert.AreEqual(Bug.State.Fixed, bug.GetCurrentState());
    }

    [TestMethod]
    public void FixInFixedStaysInFixed()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Fix();
        Assert.AreEqual(Bug.State.Fixed, bug.GetCurrentState());
    }

    [TestMethod]
    public void VerifyFromFixedReturnsToAnalysis()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();
        Assert.AreEqual(Bug.State.Analysis, bug.GetCurrentState());
    }

    [TestMethod]
    public void CloseFromAnalysisGoesToClosed()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();
        bug.Close();
        Assert.AreEqual(Bug.State.Closed, bug.GetCurrentState());
    }

    [TestMethod]
    public void ReopenFromClosedGoesToReopened()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();
        bug.Close();
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetCurrentState());
    }

    [TestMethod]
    public void RejectFromAnalysisGoesToReturned()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Reject();
        Assert.AreEqual(Bug.State.Returned, bug.GetCurrentState());
    }

    [TestMethod]
    public void ReturnFromAnalysisStaysInAnalysis()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Return();
        Assert.AreEqual(Bug.State.Analysis, bug.GetCurrentState());
    }

    [TestMethod]
    public void ReopenedCanGoBackToFixed()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();
        bug.Close();
        bug.Reopen();
        bug.Investigate();
        Assert.AreEqual(Bug.State.Fixed, bug.GetCurrentState());
    }

    [TestMethod]
    public void ClosedCanBeReopened()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();
        bug.Close();
        Assert.IsTrue(bug.GetCurrentState() == Bug.State.Closed);
        bug.Reopen();
        Assert.AreEqual(Bug.State.Reopened, bug.GetCurrentState());
    }

    [TestMethod]
    public void ReturnedCanBeReassigned()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Reject();
        bug.AssignToTeam(); // возврат в анализ
        Assert.AreEqual(Bug.State.Analysis, bug.GetCurrentState());
    }

    [TestMethod]
    public void InvalidTriggerThrowsException()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Close()); // из NewDefect нельзя Close
    }

    [TestMethod]
    public void FixTwiceInFixedStillWorks()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Fix();
        bug.Fix(); // должно пройти
        Assert.AreEqual(Bug.State.Fixed, bug.GetCurrentState());
    }

    [TestMethod]
    public void VerifyThenReopenWorks()
    {
        var bug = new Bug();
        bug.AssignToTeam();
        bug.Investigate();
        bug.Verify();   // → Analysis
        bug.Reopen();   // → Reopened
        Assert.AreEqual(Bug.State.Reopened, bug.GetCurrentState());
    }
}
