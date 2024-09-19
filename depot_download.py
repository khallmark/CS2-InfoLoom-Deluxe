import subprocess
import os
import shutil

# Updated data structure for commands
commands = [
    ("1.1.0f1-app", "download_depot 949230 949231 2240725249723137014"),
    ("1.1.1f1-app", "download_depot 949230 949231 297039206213077362 2240725249723137014"),
    ("1.1.4f1-app", "download_depot 949230 949231 9187859206915006687 297039206213077362"),
    ("1.1.5f1-app", "download_depot 949230 949231 600855205925519996 9187859206915006687"),
    ("1.1.6f1-app", "download_depot 949230 949231 1275013330304932312 600855205925519996"),
    ("1.1.7f1-app", "download_depot 949230 949231 7682216545816034430 1275013330304932312"),
    ("1.1.8f1-app", "download_depot 949230 949231 941122275615419428 7682216545816034430"),
]

def wait_for_download_completion(process):
    downloading = False
    while True:
        line = process.stdout.readline()
        if not line:
            break
        print(line.strip())
        if "Downloading depot 949231" in line:
            downloading = True
        elif downloading and line.strip():
            return

base_path = r"E:\CS2Depots\app_949230"
depot_path = os.path.join(base_path, "depot_949231")

for version, command in commands:
    # Execute the steamcmd command
    full_command = f"C:\\Users\\khallmark\\Desktop\\steamcmd\\steamcmd.exe +login kevinh456 +{command} +quit"
    process = subprocess.Popen(full_command, shell=True,
                               stdout=subprocess.PIPE, stderr=subprocess.STDOUT, universal_newlines=True)
    
    # Wait for the download to complete
    wait_for_download_completion(process)
    
    # Ensure the process has finished
    process.wait()
    
    # Create a new folder for the version
    version_folder = os.path.join(base_path, f"depot_949231_{version}")
    os.makedirs(version_folder, exist_ok=True)
    
    # Move the contents of the depot folder to the version folder
    if os.path.exists(depot_path):
        for item in os.listdir(depot_path):
            s = os.path.join(depot_path, item)
            d = os.path.join(version_folder, item)
            if os.path.isdir(s):
                shutil.copytree(s, d, dirs_exist_ok=True)
            else:
                shutil.copy2(s, d)
        print(f"Moved contents of {depot_path} to {version_folder}")
        
        # Remove the contents of the original depot folder
        for item in os.listdir(depot_path):
            s = os.path.join(depot_path, item)
            if os.path.isdir(s):
                shutil.rmtree(s)
            else:
                os.remove(s)
        print(f"Cleared contents of {depot_path}")
    else:
        print(f"Warning: {depot_path} not found after download")

print("All downloads and moves completed.")