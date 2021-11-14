# SumFiles
---
Get the sum of the values within the files/subfiles.
---
## Exercise
The function will receive as input the path to a file.
Each line of the file will contain either a number or a path to another file. 
For each file, output the file path and the sum of the numbers contained both directly in the file or in any of the sub files listed (or their sub files, etc).
## Example
If file A.txt contains:
```
3 19 B.txt 50
```
And file B.txt contains:
```
C.txt 27
```
And file C.txt contains:
```
10 2
```
The sum of A.txt and its subfiles is 111, sum for B.txt is 39, and sum for C.txt is 12. 
** Note that this is just an example. The solution should be able to handle any set of files as described in the problem statement.**