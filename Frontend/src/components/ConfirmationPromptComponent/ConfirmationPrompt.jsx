import React from 'react';
import styles from './ConfirmationPrompt.module.css'; // Create and style this module

const ConfirmationPrompt = ({ message, onConfirm, onCancel }) => {
    return (
        <div className={styles.confirmationOverlay}>
            <div className={styles.confirmationBox}>
                <p>{message}</p>
                <button className={styles.confirmButton} onClick={onConfirm}>Yes</button>
                <button className={styles.cancelButton} onClick={onCancel}>No</button>
            </div>
        </div>
    );
};

export default ConfirmationPrompt;