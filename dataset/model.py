
from sklearn import svm
from sklearn.preprocessing import StandardScaler,minmax_scale
import pickle
import pandas
import numpy as np

# 1 is time, 2 no use, 3 temperature, 4 is output,
data = pandas.read_csv("./data.csv",header=0)

# Time is divided into 3 intervals : 6-12h, 12-18h and rest of night
def normalise_list(train_array):
    normalise_list = minmax_scale(train_array,(-1,1))
    return normalise_list


# Classification: 
data = data.values.tolist()
train = [x[2:4] for x in data]

def min_max_ele():
    arr1 = [x[0] for x in train]
    arr2 = [(1 - x[1] / 1024) * 100 for x in train]
    return min(arr1),max(arr1),min(arr2),max(arr2)
print(train)
train = minmax_scale([[x[0], (1 - x[1]/1024) * 100] for x in train],(-1,1))
output = [x[4] for x in data]

clf = svm.SVC(kernel='linear')
print(train)
clf.fit(train, output)

# temp = 2*((27 - 30.8) / (35.8 - 30.8)) - 1
# soil = 2*((0.2 - 12.79)/(68.55 - 12.79)) - 1

# print(clf.predict([[temp, soil]]))


with open('./model_pkl.txt','wb') as files:
    pickle.dump(clf,files)

