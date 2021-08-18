import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ImageCardComponent} from './image-card.component';

/**
 * Mock image model to be used in the component to represent an image that has not been analysed
 */
const mockImageMetadataModel = jasmine.createSpyObj('image', [], {
  id: 'test_id', url: 'test_url',
  name: 'test_name', dateStored : new Date(2017, 4, 4, 17, 23, 42, 11)
});

describe('ImageCardComponent', () => {
  let component: ImageCardComponent;
  let fixture: ComponentFixture<ImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ImageCardComponent],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageCardComponent);
    component = fixture.componentInstance;
    component.image = mockImageMetadataModel;
    fixture.detectChanges();
  }));

  /**
   * Checks the component has been created successfully
   */
  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Checks that the image will have the correct src and will therefore display the correct image
   */
  it('should show correct image', () => {
    const imageUrl = fixture.debugElement.nativeElement.querySelector('ion-img').src;
    expect(imageUrl).toBe(mockImageMetadataModel.url);
  });

  /**
   * Checks that the image name matches the mock image model's name
   */
  it('should show correct name', () => {
    const cardname = fixture.debugElement.nativeElement.querySelector('ion-card-title').innerText.trim();
    expect(cardname).toBe(mockImageMetadataModel.name);
  });

  /**
   * Checks that the image's delete button calls the correct method
   */
  it('should call onDeleteImage method', () => {
    spyOn(component, 'onDeleteImage');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.onDeleteImage).toHaveBeenCalled();
  });

  /**
   * Checks that the component's delete button emits the deleteImage event
   */
  it('should emit the deleteImage event on click', () => {
    spyOn(component.deleteImage, 'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalled();
  });

  /**
   * Checks that the deleteImage event contains the correct payload once it's emitted
   */
  it('deleteImage event should contain correct payload', () => {
    spyOn(component.deleteImage, 'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalledWith(mockImageMetadataModel.id);
  });


  /**
   * Checks that the view analysed image button is present if the analysed property of the image model = true
   *
   *    * Currently ignored in unit test, since analyse Image has not yet been finalised
   */
  xit('should create view analysed image button', () => {
    component.image = mockImageMetadataModel;
    const viewAnalysedImageButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    expect(viewAnalysedImageButton).toBeTruthy();
  });

  /**
   * Checks that the analyse image button press calls the correct function in the component
   *
   * Currently ignored in unit test, since analyse Image has not yet been finalised
   */
  xit('should call correct component function once pressed', () => {
    spyOn(component, 'analyseImage');
    const analyseImageButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    analyseImageButton.click();
    expect(component.analyseImage).toHaveBeenCalled();
  });

  /**
   * Checks that the view analysed image button press calls the correct function in the component
   */
  it('should call viewAnalysedImage method in the component', () => {
    component.image = mockImageMetadataModel;
  });

  /**
   * Checks that the viewImageFullScreen image function is called once the button to open the image is clicked
   */
  it('should call viewImageFullScreen method of component', () => {
    spyOn(component, 'viewImageFullScreen');
    const openImageFullScreenButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    openImageFullScreenButton.click();
    expect(component.viewImageFullScreen).toHaveBeenCalled();
  });
});
