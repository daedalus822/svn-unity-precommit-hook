# SVN Unity Pre-commit Hook
Client pre-commit hook that will ensure you commit .meta files

1. Put folder wherever you want on your HDD where you won't delete it.

2. Open TortoiseSVN Settings

3. Go to Hook Scripts

4. Click Add, Change hook type to pre-commit. For working copy path you must put it to a Unity project under the Assets folder!
Under Command Line to Execute, navigate to where you deployed the folder and click on UnityMetaChecker.exe

5. Click all three check boxes (wait for script to finish, hide the script while running, and always execute the script)

6. Everytime you do an ADD under the Assets folder it will require you to push a .meta file along with the file.
If you are just modifying files it will not do a check on these files.
