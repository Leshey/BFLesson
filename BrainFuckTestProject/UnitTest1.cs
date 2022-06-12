using BrainFuck.BF;
using BrainFuck.IO;
using BrainFuck.Interfaces.IO;
using BrainFuck.Interfaces.BF;
using BrainFuck.Interfaces.Menus;
using Moq;
using Xunit;
using System.IO;


namespace BrainFuck;

public class DataOperationTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(69)]
    public void NextCharValueTest(int newValue)
    {
        // arrange
        var repository = new Repository("");
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);

        var expectedCurrent = newValue + 1;
        repository.Memory[repository.Current] = (char)newValue;

        // act
        brainFuckFunction.NextCharValue();
        var actual = repository.Memory[repository.Current];

        // assert
        Assert.Equal(expectedCurrent, actual);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(69)]
    public void PreviousCharValueTest(int newValue)
    {
        // arrange
        var repository = new Repository("");
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);

        var expectedCurrent = newValue - 1;
        repository.Memory[repository.Current] = (char)newValue;

        // act
        brainFuckFunction.PreviousCharValue();
        var actual = repository.Memory[repository.Current];

        // assert
        Assert.Equal(expectedCurrent, actual);
    }

    [Theory]
    [InlineData("}")]
    [InlineData("<")]
    [InlineData("/")]
    public void DisplayCellValueTest(string newValue)
    {
        // arrange
        var repository = new Repository("");
        var mockCursorWrapper = new Mock<ICursorWrapper>();
        var mockTextReader = new Mock<TextReader>();
        var mockTextWriter = new Mock<TextWriter>();
        var called = false;
        mockTextWriter.Setup(x => x.Write(newValue)).Callback(() => called = true);

        var inputOutput = new InputOutput(mockTextReader.Object, mockTextWriter.Object, mockCursorWrapper.Object);
        var brainFuckFunction = new BrainFuckFunction(repository, inputOutput);

        repository.Memory[0] = newValue[0];

        // act
        brainFuckFunction.DisplayCellValue();

        // assert
        Assert.True(called);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(69)]
    [InlineData(121)]
    public void NextCellTest(int newValue) 
    {
        // arrange
        var repository = new Repository("");
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);

        var expectedCurrent = newValue + 1;
        repository.Current = newValue;

        // act
        brainFuckFunction.NextCell();
        var actual = repository.Current;

        // assert
        Assert.Equal(expectedCurrent, actual);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(69)]
    [InlineData(121)]
    public void PreviousCellTest(int newValue)
    {
        // arrange
        var repository = new Repository("");
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);

        var expectedCurrent = newValue - 1;
        repository.Current = newValue;

        // act
        brainFuckFunction.PreviousCell();
        var actual = repository.Current;

        // assert
        Assert.Equal(expectedCurrent, actual);
    }

    [Theory]
    [InlineData('}', "}")]
    [InlineData('+', "+")]
    [InlineData('_', "_")]
    public void InputValueInCellTest(char expectedValue, string newValue) 
    {
        // arrange
        var repository = new Repository("");
        var mockCursorWrapper = new Mock<ICursorWrapper>();
        var mockTextReader = new Mock<TextReader>();
        mockTextReader.Setup(x => x.ReadLine()).Returns(newValue);
        var mockTextWriter = new Mock<TextWriter>();
        
        var inputOutput = new InputOutput(mockTextReader.Object, mockTextWriter.Object, mockCursorWrapper.Object); //Object ı‡ÌËÚ ‚ ÒÂ·Â Â‡ÎËÁ‡ˆË˛
        var brainFuckFunction = new BrainFuckFunction(repository, inputOutput);

        // act
        brainFuckFunction.InputValueInCell();
        var actual = repository.Memory[0];

        // assert
        mockTextReader.Verify(x => x.ReadLine(), Times.Once);
        Assert.Equal(expectedValue, actual);
    }

    [Theory]
    [InlineData("[]", 1)]
    [InlineData("+++++[++++[++++++[][][][][][][][][][][]][+++++++][][][][]][][][][][][][][]++]+++++]", 76)]
    [InlineData("[++++[++++++++]++++++++[++++[[[+++++++++++[+++++++]]]--------------]]++++]", 73)]
    public void IfZeroNextTest(string newProgram, int expectedValue)
    {
        // arrange
        var repository = new Repository(newProgram);
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);
        repository.Memory[repository.Current] = (char)0;
        var PositionNumber = 0;
       
        // act
        var actual = brainFuckFunction.IfZeroNext(PositionNumber, repository.Program);
        
        // assert
        Assert.Equal(expectedValue, actual);
    }

    [Theory]
    [InlineData("[++++[++++++++]+++++++]", 22)]
    [InlineData("[[[]]]", 5)]
    [InlineData("[++++[++++++++]++++++++[++++[[[+++++++++++[+++++++]]]--------------]]++++]", 73)]
    public void IfNoZeroBackTest(string newProgram, int newPositionNumber)
    {
        // arrange
        var repository = new Repository(newProgram);
        var mockInputOutput = new Mock<IInputOutput>();
        var brainFuckFunction = new BrainFuckFunction(repository, mockInputOutput.Object);
        repository.Memory[repository.Current] = (char)1;
        var expectedCurrent1 = 0;

        // act
        var actual1 = brainFuckFunction.IfNoZeroBack(newPositionNumber, repository.Program);

        // assert
        Assert.Equal(expectedCurrent1, actual1);
    }

    [Theory]
    [InlineData("+", "NextCharValue")]
    [InlineData("-", "PreviousCharValue")]
    [InlineData(".", "DisplayCellValue")]
    [InlineData(">", "NextCell")]
    [InlineData("<", "PreviousCell")]
    [InlineData(",", "InputValueInCell")]
    [InlineData("[", "IfZeroNext")]
    [InlineData("]", "IfNoZeroBack")]
    public void Enum—odeBrainFuckTest(string brainFuckCode, string expectedName)
    {
        // arrange
        var mockBrainFuckFunction = new Mock<IBrainFuckFunction>();
        var actualName = "invalidName";

        mockBrainFuckFunction.Setup(x => x.NextCharValue()).Callback(() => actualName = "NextCharValue");
        mockBrainFuckFunction.Setup(x => x.PreviousCharValue()).Callback(() => actualName = "PreviousCharValue");
        mockBrainFuckFunction.Setup(x => x.DisplayCellValue()).Callback(() => actualName = "DisplayCellValue");
        mockBrainFuckFunction.Setup(x => x.NextCell()).Callback(() => actualName = "NextCell");
        mockBrainFuckFunction.Setup(x => x.PreviousCell()).Callback(() => actualName = "PreviousCell");
        mockBrainFuckFunction.Setup(x => x.InputValueInCell()).Callback(() => actualName = "InputValueInCell");
        mockBrainFuckFunction.Setup(x => x.IfZeroNext(0, brainFuckCode)).Callback(() => actualName = "IfZeroNext");
        mockBrainFuckFunction.Setup(x => x.IfNoZeroBack(0, brainFuckCode)).Callback(() => actualName = "IfNoZeroBack");

        var dataOperations = new DataOperations(mockBrainFuckFunction.Object);

        // act
        dataOperations.Enum—odeBrainFuck(brainFuckCode);

        // assert
        Assert.Equal(expectedName, actualName);
    }
}


