import os
import os.path
import sys
import subprocess
import math

str="D:\cachecsv\\"
pydir="C:\Python27\python"

def compare(case ,arguments=[]):
    #print str+os.path.basename(case).replace("xlsx","csv")
   # subprocess.call(["C:\Python27\python","./xlsx2csv.py"] + arguments + [case] +[str+os.path.basename(case).replace("xlsx","csv")])
    subprocess.call([pydir,"./xlsx2csv.py"] + arguments + [case] +[case.replace("xlsx","csv")])


def GetFileFromThisRootDir(dir,ext = None):
  allfiles = []
  needExtFilter = (ext != None)
  for root,dirs,files in os.walk(dir):
    for filespath in files:
      filepath = os.path.join(root, filespath)
      extension = os.path.splitext(filepath)[1][1:]
      if needExtFilter and extension in ext:
        allfiles.append(filepath)
      elif not needExtFilter:
        allfiles.append(filepath)
  return allfiles

def deleteDir(dir):
    mydirs=[]
    for root,dirs,files in os.walk(dir):
        for dir1 in dirs:
            if dir1.find(".csv")>0:
                mydirs.append(os.path.join(root, dir1))
    return mydirs

files=GetFileFromThisRootDir("./",['xlsx'])

num=len(files)
#print(os.path.abspath('.'))
for i in range(0,num):
    compare(files[i],["-a"])
csvflies=GetFileFromThisRootDir("./",['csv'])
path1=os.path.abspath('.')

for csv in csvflies:
    path2=os.path.abspath(csv)
    path3=path2.replace(path1,"")
    src=open(csv)
    str="./Data"+path3.replace("csv","txt")
    #print(str)
    dirname=os.path.dirname(str)
   # print dirname
    if dirname.find(".txt")>0:
        index=dirname.rfind("\\")
        dirname=dirname[0:index]
    #print dirname                       
    if not os.path.exists(dirname) :
        os.makedirs(dirname)
    des=file(os.path.join(dirname, os.path.basename(str)),"w+")
    des.write(src.read())
    des.close()
    src.close()
    os.remove(csv)
dirs=deleteDir(path1)
for dir in dirs:
    #print dir
    os.rmdir(dir)
print "ok"




