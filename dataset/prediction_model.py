from pandas import read_csv
from sklearn import svm
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import train_test_split,StratifiedKFold
import numpy as np
import pickle
import math
# Perhaps we should consider linear regression model first

# Return the group number of the time stamp.
# 6-12h for 1, 12-18h for 2 and the night is for 3

df = read_csv('data.csv')
X = df[['Temp','Moisture','Hour']]
Y = df['NextDayInMinute']
Z = df['Decision']

X = X.values.tolist()
Y = Y.values.tolist()
Y = [math.log(i+0.001) for i in Y]
Z = Z.values.tolist()

X_train,X_test,Y_train,Y_test = train_test_split(X,Y,shuffle="True",train_size=0.75)

regressionModel = LinearRegression().fit(X_train,Y_train)
print("mse train:", mean_squared_error(Y_train, regressionModel.predict(X_train)))
print("mse test:", mean_squared_error(Y_test,regressionModel.predict(X_test)))

print(regressionModel.predict([[27,500,20]]))
X = [[i[0],float(1 - i[1]/1024)*100] for i in X]
X1 = []
for i,j in enumerate(X):
    if Z[i] == 1:
        X1.append(X[i])


for i in range(20):
    X.append([X1[i][0] + np.random.uniform(-1,1),X1[i][1] + np.random.uniform(-10,10)])
    X.append([X1[i][0] + np.random.uniform(-1,1),X1[i][1] + np.random.uniform(-10,10)])
    Y.append(0)
    Y.append(0)
    Z.append(1)
    Z.append(1)

X_train, X_test, Y_train, Y_test = train_test_split(X,Z,test_size=0.33,stratify=Z,random_state=40,shuffle=True)

classificationModel = svm.SVC(kernel='linear')
classificationModel.fit(X_train,Y_train)

def getAccurary():
    return classificationModel.score(X_test,Y_test)

print("Accuracy: " ,getAccurary())



with open('./classification_model_pkl.txt','wb') as files:
    pickle.dump(classificationModel,files)
with open('./regression_model_pkl.txt','wb') as files:
    pickle.dump(regressionModel,files)





# rawY = rawX.pop(5)
# print(rawY)
# print(rawX)


# rawY = rawX.pop(4)




