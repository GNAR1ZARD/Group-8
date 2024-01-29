import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
from datetime import datetime

# Load data into a DataFrame
df = pd.read_csv('/home/toefoo/_code/groupProject/repo_mining/data/rootbeer_authors_dates.csv')

# Convert 'Date' column to datetime objects and sort by date
df['Date'] = pd.to_datetime(df['Date'])
df.sort_values('Date', inplace=True)

# Calculate the number of weeks since the first commit for each entry
start_date = df['Date'].min()
df['weeks'] = df['Date'].apply(lambda x: (x - start_date).days // 7)

# Assign an index to each unique filename
df['file_index'] = df['Filename'].astype('category').cat.codes

# Assign a unique color to each author
colors = plt.cm.jet(np.linspace(0, 1, df['Author'].nunique()))
color_dict = {author: color for author, color in zip(df['Author'].unique(), colors)}

# Plot each author's data points with their respective color
fig, ax = plt.subplots()
for author, group in df.groupby('Author'):
    ax.scatter(group['file_index'], group['weeks'], s=50, color=color_dict[author], label=author)

# Some fancy labels
ax.set_xlabel('File Index')
ax.set_ylabel('Weeks Since First Commit')
ax.set_title('File Touches over Time by Author')
ax.legend(title='Author', bbox_to_anchor=(1.05, 1), loc='upper left')
plt.grid(True)

# Output the plot
plt.show()
