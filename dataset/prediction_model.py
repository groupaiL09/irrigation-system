from pandas import read_csv
from sklearn.linear_model import LinearRegression
import pickle
# Perhaps we should consider linear regression model first

# Return the group number of the time stamp.
# 6-12h for 1, 12-18h for 2 and the night is for 3

df = read_csv('data.csv')
X = df[['Temp','Moisture','Hour']]
Y = df['NextDayInMinute']


X = X.values.tolist()
Y = Y.values.tolist()


regressionModel = LinearRegression().fit(X,Y)
# print(regressionModel.predict([[32.5,780,23]]))

with open('./regression_model_pkl.txt','wb') as files:
    pickle.dump(regressionModel,files)





# rawY = rawX.pop(5)
# print(rawY)
# print(rawX)


# rawY = rawX.pop(4)




