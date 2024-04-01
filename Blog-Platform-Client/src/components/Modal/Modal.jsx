import React from 'react'
import classes from './Modal.module.css'

const Modal = ({children, visible, setVisible}) => {

    const rootDivClass = [classes.modal_background_div]
    if (visible) {
        rootDivClass.push(classes.active);
    }

  return (
    <div className={rootDivClass.join(' ')}
    onClick={() => {setVisible(false)}}
    >
        <div className={classes.modal_content_div}
        onClick={(e) => {e.stopPropagation()}}
        >
            {children}
        </div>
    </div>
  )
}

export default Modal