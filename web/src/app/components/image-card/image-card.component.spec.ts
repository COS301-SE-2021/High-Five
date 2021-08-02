import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ImageCardComponent} from './image-card.component';

/**
 * Mock image model to be used in the component to represent an image that has not been analysed
 */
const mockImageModelNotAnalysed = jasmine.createSpyObj('image', [], {
  id: 'test_id', url: 'test_url',
  title: 'test_title', analysed: false, analysedId: 'test_analysedId'
});


/**
 * Mock image model to be used in the component to represent an image that has been analysed
 */
const mockImageModelAnalysed = jasmine.createSpyObj('image', [], {
  id: 'test_id', url: 'test_url',
  title: 'test_title', analysed: false, analysedId: 'test_analysedId'
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
    component.image = mockImageModelNotAnalysed;
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
    expect(imageUrl).toBe(mockImageModelNotAnalysed.url);
  });

  /**
   * Checks that the image tittle matches the mock image model's title
   */
  it('should show correct title', () => {
    const cardTitle = fixture.debugElement.nativeElement.querySelector('ion-card-title').innerText.trim();
    expect(cardTitle).toBe(mockImageModelNotAnalysed.title);
  });

  /**
   * Checks that the image's delete button calls the correct method
   */
  it('should call onDeleteImage method', () => {
    spyOn(component, 'onDeleteImage');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[2];
    deleteButton.click();
    expect(component.onDeleteImage).toHaveBeenCalled();
  });

  /**
   * Checks that the component's delete button emits the deleteImage event
   */
  it('should emit the deleteImage event on click', () => {
    spyOn(component.deleteImage, 'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[2];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalled();
  });

  /**
   * Checks that the deleteImage event contains the correct payload once it's emitted
   */
  it('deleteImage event should contain correct payload', () => {
    spyOn(component.deleteImage, 'emit');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[2];
    deleteButton.click();
    expect(component.deleteImage.emit).toHaveBeenCalledWith(mockImageModelNotAnalysed.id);
  });


  /**
   * Checks that the view analysed image button is present if the analysed property of the image model = true
   */
  it('should create view analysed image button', () => {
    component.image = mockImageModelAnalysed;
    const viewAnalysedImageButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    expect(viewAnalysedImageButton).toBeTruthy();
  });

  /**
   * Checks that the analyse image button press calls the correct function in the component
   */
  it('should call correct component function once pressed', () => {
    spyOn(component, 'analyseImage');
    const analyseImageButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[0];
    analyseImageButton.click();
    expect(component.analyseImage).toHaveBeenCalled();
  });

  /**
   * Checks that the view analysed image button press calls the correct function in the component
   */
  it('should call viewAnalysedImage method in the component', () => {
    mockImageModelNotAnalysed.analysed = true;
    component.image = mockImageModelNotAnalysed;
  });


  /**
   * Checks that the analyse image button press changes the analysed value of the image model, the component's image
   * object needs to be set explicitly here as the spy objects that are defined at the start of the unit tests, will
   * not be able to be manipulated (their properties can't be changed) causing the test to fail.
   */
  it('should change analysed value of image model', () => {
    component.image = {
      id: 'test_id', url: 'test_url',
      title: 'test_title', analysed: false, analysedId: 'test_analysedId'
    };
    const analysed = component.image.analysed;
    component.analyseImage();
    expect(component.image.analysed).toBe(!analysed);
  });

  it('should call viewImageFullScreen method of component', () => {
    spyOn(component, 'viewImageFullScreen');
    const openImageFullScreenButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    openImageFullScreenButton.click();
    expect(component.viewImageFullScreen).toHaveBeenCalled();
  });
});
