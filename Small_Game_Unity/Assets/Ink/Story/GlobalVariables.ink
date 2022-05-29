// This script contains utilities and variables accessable to all story scripts

EXTERNAL addMemory(memoryName)
EXTERNAL removeMemory(memoryName)
EXTERNAL checkMemory(memoryName)
EXTERNAL getDate()
EXTERNAL getTime()

VAR currentDay = 0
VAR currentTime = ""
LIST lastAction = gameConsole, bed, book, guitar, computer, smartphone