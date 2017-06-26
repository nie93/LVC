import os, csv

def clear_log_xml_files(dir):
    listdir = os.listdir(dir)
    for item in listdir:
        if (item.startswith('CtrlDecisionLog_') & item.endswith('.xml')):
            os.remove(os.path.join(dir, item))

if __name__ == '__main__':
    dirPath = os.path.dirname(os.path.realpath(__file__))
    clear_log_xml_files(dirPath)


