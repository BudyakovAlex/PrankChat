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

api_key = '7b566fa03da071be83f11f805c5cd479'
project_id = 296029

#App Android 
app_android_ru = getDownloadUrl(api_key, project_id, 'ru', 'android_strings') 
print (app_android_ru)
downloadFile(app_android_ru, '../src/Droid/Resources/values/strings.xml');

#App iOS
app_ios_ru = getDownloadUrl(api_key, project_id, 'ru', 'apple_strings') 
print (app_ios_ru)
downloadFile(app_ios_ru, '../src/iOS/Resources/ru.lproj/Localizalbe.strings');

#Core
core_ru = getDownloadUrl(api_key, project_id, 'ru', 'resx') 
print (core_ru)
downloadFile(core_ru, '../src/Core/Presentation/Localization/Resources.resx');



