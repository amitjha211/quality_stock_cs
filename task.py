import sys
import os
import shutil




def chkArg(cmd="",iIndex=0):
    if cmd in sys.argv:
        return True
    else:
        return False

def join_file(config):
    #os.getcwd() + "\\" 

    rootPath = ""
    fresult = open( rootPath + config['out'],"w+")

    for fileName in config['input']:
        fInput = open(rootPath + fileName,"r")

        fresult.writelines("")
        fresult.writelines(fInput.readlines())
        fresult.write("")
        #closing the file..
        fInput.close()
    

    fresult.flush()
    fresult.close()

def join_files(*args):
    for r in args:
        join_file(r)


def copyAll(sourceFolder,destinationFolder):
    for filename in os.listdir(sourceFolder):
        shutil.copy(sourceFolder + filename, destinationFolder)
        print(filename)




