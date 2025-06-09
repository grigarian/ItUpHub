import React from "react";
import { Droppable } from "@hello-pangea/dnd";
import TaskCard from "./TaskCard";

const Column = ({ status, issues, onEditClick }) => {
  return (
    <Droppable droppableId={status}>
      {(provided) => (
        <div
          ref={provided.innerRef}
          {...provided.droppableProps}
          className="flex flex-col bg-white/90 backdrop-blur-lg rounded-2xl p-4 w-80 min-h-[200px] shadow-md border border-gray-100"
        >
          <h3 className="text-lg font-semibold text-gray-800 mb-4 px-2 py-1 border-b border-gray-200">
            {status}
          </h3>

          <div className="space-y-3">
            {issues.map((issue, index) => (
              <TaskCard
                key={issue.id}
                issue={issue}
                index={index}
                onClick={() => onEditClick(issue)}
              />
            ))}
            {provided.placeholder}
          </div>
        </div>
      )}
    </Droppable>
  );
};

export default Column;