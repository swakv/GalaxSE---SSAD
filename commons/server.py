from flask import Flask, request
import os
import platform

app = Flask(__name__)

@app.route("/api", methods = ["GET"])
def main():
    args = request.args.get("q")
    f = open("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt","w+")
    f.write(args)

    os.system("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/galaxy.app")

    return ""