#!/usr/bin/env python

# package to install : 
# $ pip install requests or sudo easy_install -U requests
# for package 'requests' you may need also to install tornado & nose
# $ sudo easy_install tornado
# $ sudo easy_install nose

# run script: python poeditor_export.py

# also you can create Alias to specify wich version of interpreter to use
# which -a python python2 python2.7 python3 python3.6
# python=/usr/local/bin/python3.6

# to create a virtualenv:
# virtualenv ENV
# source ENV/bin/activate
# deactivate (to go out of the venv)

import requests
import os
import urllib
import sys

if sys.version_info[0] >= 3:
	from urllib.request import urlretrieve
else:
    # Not Python 3 - today, it is most likely to be Python 2
    # But note that this might need an update when Python 4
    # might be around one day
	from urllib import urlretrieve

def getDownloadUrl(api_key, project_id, export_lang, export_type):
	result = requests.post('https://poeditor.com/api/', data = {'api_token':api_key, 'action':'export', 'id':project_id, 'language':export_lang, 'type':export_type})
	print (result)
	return result.json().get('item')

def downloadFile(url, path):
	urlretrieve(url, path)

api_key = '6a76e9420945b04d5418bb04eea679a1'
project_id = 296029

#App Android
app_android_en = getDownloadUrl(api_key, project_id, 'en-us', 'android_strings') 
print (app_android_en)
downloadFile(app_android_en, '../src/Droid/Resources/values/strings.xml');

#App iOS
app_ios_en = getDownloadUrl(api_key, project_id, 'en-us', 'apple_strings') 
print (app_ios_en)
downloadFile(app_ios_en, '../src/iOS/Presentation/Localization/en.lproj/Localizalbe.strings');

#Core
core_en = getDownloadUrl(api_key, project_id, 'en-us', 'resx') 
print (core_en)
downloadFile(core_en, '../src/Core/Presentation/Localization/Resources.resx');



