# Command line parser demo

This folder contains a .Net console application project that demonstrates the usage of the command line arguments parser.

## Build the solution
1. Open the solution file ParseTheArgs.sln in the root folder in Visual Studio.
2. In the Visual Studio menu click on "Build" and then on "Build solution".
3. Open a command line in the folder ParseTheArgs.Demo\bin\Debug\netcoreapp3.1.
4. Try out one of the commands listed under "Example usage".

# Example usage
##### Display general help
> Toolbox

##### Display help of the date command
> Toolbox help date

##### Download and print out the content of a website
> Toolbox -w http://www.microsoft.com

##### Query a search engine and print out the result
> Toolbox --search "What is life"

##### Display the current date and time
> Toolbox date

##### Display the current date and time in UTC
> Toolbox date --utc

##### Display a specific date
> Toolbox date --date "31.12.2020 23:59:59"

##### Add on offset to a date
> Toolbox date --date "01.01.2020 10:00:00" --offset 10:15:00

##### Calculate the difference between two dates
> Toolbox date --date "01.01.2020 10:00:00" --differenceTo "15.02.2020 20:00:00"

##### Convert a Guid to a sequence of bytes:
> Toolbox convertGuid --guid ac23ddb2-6e34-46a9-80f4-9d6fe6f87558 --to Bytes

##### Convert a Guid to a big integer:
> Toolbox convertGuid --guid ac23ddb2-6e34-46a9-80f4-9d6fe6f87558 --to BigInteger

##### Find the longest word of a list of words
> Toolbox longestWord --words cat dog fish crocodile

##### Replace the word cat with dog in a text file
> Toolbox fileReplace --in Cat.txt --out Dog.txt --pattern cat --replaceWith dog --display

##### Replace cats and humans in a text file
> Toolbox fileReplace --in Cat.txt --out Dog.txt --pattern "\b(cat(s)?|human(s?))\b" --replaceWith dog$2$3 --regex --display
