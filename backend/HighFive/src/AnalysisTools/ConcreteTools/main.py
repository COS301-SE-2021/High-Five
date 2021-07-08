import numpy as np
import cv2
import cvlib as cvlib
import matplotlib.pyplot as plt


def draw(img, bbox, labels):
    for i, label in enumerate(labels):

        cv2.rectangle(img, (bbox[i][0],bbox[i][1]), (bbox[i][2],bbox[i][3]), (0,255,0), 6)

        cv2.putText(img, label, (bbox[i][0],bbox[i][1]-10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0,255,0), 6)

        cv2.putText(img, "Car count: "+str(label.count('car')), (10, 25), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 255, 0), 2)
    return img


def analyse(bytes):
    # import numpy as np
    # import cv2
    # import cvlib as cvlib
    # import matplotlib.pyplot as plt

    np_arr = np.frombuffer(bytes, np.uint8)
    im = cv2.imdecode(np_arr, flags=1)

    bbox, label, conf = cvlib.detect_common_objects(im)

    output_image = draw(im, bbox, label)

    plt.imshow(output_image)
    plt.show()

    return cv2.imencode('.jpg', im)[1]


def main():
    fd = open('cars-driving.jpg', "rb")
    img_str = fd.read()
    fd.close()
    print(analyse(img_str))


main()
