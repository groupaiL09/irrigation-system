import pickle


from model import min_max_ele
with open ('./model_pkl.txt','rb') as f:
    model = pickle.load(f)

min_temp, max_temp, min_moist, max_moist = min_max_ele()

new_data = [33.3, 893]
new_data[0] = ((new_data[0] - min_temp) / (max_temp - min_temp))*2 - 1 
new_data[1] = ((new_data[1] - min_moist) / (max_moist - min_moist))*2 - 1
print(new_data)
print(model.predict([new_data]))