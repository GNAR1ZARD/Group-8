import json
import requests
import csv
import os
from collections import defaultdict

# Create the data directory if it does not exist
data_directory = "/home/toefoo/_code/groupProject/repo_mining/data"
if not os.path.exists(data_directory):
    os.makedirs(data_directory)


# GitHub Authentication function
def github_auth(url, lsttoken, ct):
    jsonData = None
    try:
        ct = ct % len(lsttoken)
        headers = {"Authorization": "Bearer {}".format(lsttoken[ct])}
        request = requests.get(url, headers=headers)
        jsonData = json.loads(request.content)
        ct += 1
    except Exception as e:
        print(e)
    return jsonData, ct


# Get the programming languages used in the repo
def get_repo_languages(repo, lsttokens, ct):
    languages_url = f"https://api.github.com/repos/{repo}/languages"
    languages_data, ct = github_auth(languages_url, lsttokens, ct)
    return list(languages_data.keys()), ct


# Check if the file is a source file based on its extension
def is_source_file(filename, languages):
    # Mapping of languages to file extensions
    extension_mapping = {
        "Python": [".py"],
        "JavaScript": [".js"],
        "Java": [".java"],
        # Add other languages?
    }
    file_extension = os.path.splitext(filename)[1]
    for lang in languages:
        if file_extension in extension_mapping.get(lang, []):
            return True
    return False


# Modified countfiles function to gather only source files
def countfiles(dictfiles, lsttokens, repo):
    ipage = 1  # URL page counter
    ct = 0  # Token counter

    # Fetch the languages used in the repo
    languages, ct = get_repo_languages(repo, lsttokens, ct)

    try:
        # Loop through all the commit pages until the last returned empty page
        while True:
            spage = str(ipage)
            commits_url = (
                f"https://api.github.com/repos/{repo}/commits?page={spage}&per_page=100"
            )
            json_commits, ct = github_auth(commits_url, lsttokens, ct)

            # Break out of the while loop if there are no more commits in the pages
            if not json_commits:
                break

            # Iterate through the list of commits in the page
            for sha_object in json_commits:
                sha = sha_object["sha"]
                # For each commit, use the GitHub commit API to extract the files touched by the commit
                sha_url = f"https://api.github.com/repos/{repo}/commits/{sha}"
                sha_details, ct = github_auth(sha_url, lsttokens, ct)

                # Check if the request was successful
                if sha_details is not None:
                    files_json = sha_details.get("files", [])
                    for filename_obj in files_json:
                        filename = filename_obj["filename"]
                        # Filter out non-source files
                        if is_source_file(filename, languages):
                            dictfiles[filename].append(
                                {
                                    "sha": sha,
                                    "author": sha_object["commit"]["author"]["name"],
                                    "date": sha_object["commit"]["author"]["date"],
                                }
                            )
                            print(filename)
            ipage += 1
    except Exception as e:
        print(f"Error receiving data: {e}")
        exit(0)


# Main execution
repo = "scottyab/rootbeer"  # Example repo
lstTokens = ["Token ID"]  # DONT FORGET TO DELETE
dictfiles = defaultdict(list)
countfiles(dictfiles, lstTokens, repo)

# Output
file = repo.split("/")[1]
fileOutput = os.path.join(data_directory, f"{file}_authors_dates.csv")
rows = ["Filename", "Author", "Date"]

with open(fileOutput, "w", newline="") as fileCSV:
    writer = csv.writer(fileCSV)
    writer.writerow(rows)

    for filename, touches in dictfiles.items():
        for touch in touches:
            writer.writerow([filename, touch["author"], touch["date"]])

# Close the CSV file and print the file with the most touches
bigcount = None
bigfilename = None
for filename, touches in dictfiles.items():
    count = len(touches)
    if bigcount is None or count > bigcount:
        bigcount = count
        bigfilename = filename

print(f"The file {bigfilename} has been touched {bigcount} times.")

# Completion message
print("Data collection complete. The CSV file has been created.")
